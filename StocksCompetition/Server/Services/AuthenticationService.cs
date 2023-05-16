using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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

    public async Task<JwtResponse> LogIn(LogInForm logInForm)
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
        
        
        JwtSecurityToken token = GenerateToken(user);
        var refreshToken = new RefreshToken(user.Id);
        await _context.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        
        return new JwtResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            token.ValidTo,
            refreshToken.Token.ToString()
        );
    }

    public async Task<JwtResponse> SignUp(SignUpForm signUpForm)
    {
        ApplicationUser? existingUser = await _userManager.FindByDiscordUsernameAsync(signUpForm.DiscordUsername);
        if (existingUser is not null)
        {
            throw new UserAlreadyExistsException();
        }
        
        var user = new ApplicationUser(signUpForm);
        await _context.Users.AddAsync(user);
        
        user.PasswordHash = _passwordHasher.HashPassword(user, user.Password);
        user.SecurityStamp = Guid.NewGuid().ToString();
        await _context.SaveChangesAsync();

        JwtSecurityToken token = GenerateToken(user);
        var refreshToken = new RefreshToken(user.Id);
        await _context.AddAsync(refreshToken);
        await _context.SaveChangesAsync();
        
        return new JwtResponse(
            new JwtSecurityTokenHandler().WriteToken(token),
            token.ValidTo,
            refreshToken.Token.ToString()
        );
    }

    public async Task<JwtResponse> RefreshToken(string refreshToken, string userId)
    {
        RefreshToken token = await _context.RefreshTokens
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Token == Guid.Parse(refreshToken) && t.UserId == userId)
            ?? throw new LogInException();

        var newRefreshToken = new RefreshToken(userId);
        await _context.RefreshTokens.AddAsync(newRefreshToken);
        _context.RefreshTokens.Remove(token);
        await _context.SaveChangesAsync();
        
        JwtSecurityToken responseToken = GenerateToken(token?.User ?? throw new LogInException());
        return new JwtResponse(
            new JwtSecurityTokenHandler().WriteToken(responseToken),
            responseToken.ValidTo,
            newRefreshToken.Token.ToString()
        );
    }

    private JwtSecurityToken GenerateToken(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
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
