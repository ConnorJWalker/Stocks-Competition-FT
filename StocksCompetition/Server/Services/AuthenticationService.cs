using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using StocksCompetition.Server.Entities;
using StocksCompetition.Server.Models;
using StocksCompetition.Shared;

namespace StocksCompetition.Server.Services;

public class AuthenticationService
{
    private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
    private readonly ServerDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationService(IPasswordHasher<ApplicationUser> passwordHasher, ServerDbContext context, IConfiguration configuration, UserManager<ApplicationUser> userManager)
    {
        _passwordHasher = passwordHasher;
        _context = context;
        _configuration = configuration;
        _userManager = userManager;
    }

    public async Task<JwtSecurityToken> LogIn(LogInForm logInForm)
    {
        ApplicationUser? user = await _userManager.FindByDiscordUsernameAsync(logInForm.DiscordUsername);
        if (user is null)
        {
            throw new LogInException();
        }

        PasswordVerificationResult result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, logInForm.Password);
        if (result == PasswordVerificationResult.Failed)
        {
            throw new LogInException();
        }

        return GenerateToken(user);
    }

    public async Task<JwtSecurityToken> SignUp(SignUpForm signUpForm)
    {
        ApplicationUser? existingUser = await _userManager.FindByDiscordUsernameAsync(signUpForm.DiscordUsername);
        if (existingUser is not null)
        {
            throw new UserAlreadyExistsException();
        }
        
        var user = new ApplicationUser(signUpForm);
        await _context.Users.AddAsync(user);
        
        user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);
        user.SecurityStamp = new Guid().ToString();
        await _context.SaveChangesAsync();

        return GenerateToken(user);
    }

    private JwtSecurityToken GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim("ProfilePicture", user.ProfilePicture),
            new Claim("DisplayName", user.DisplayName),
            new Claim("DiscordUsername", user.DiscordUsername),
            new Claim("DisplayColour", user.DisplayColour)
        };

        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
        return new JwtSecurityToken(
            issuer: _configuration["Jwt:ValidIssuer"],
            expires: DateTime.Now.AddMinutes(20),
            claims: claims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
    }
}

public class LogInException : Exception { }

public class UserAlreadyExistsException : Exception { }
