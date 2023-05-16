using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksCompetition.Server.Entities;
using StocksCompetition.Server.Services;
using StocksCompetition.Shared;

namespace StocksCompetition.Server.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthenticationController : Controller
{
    private readonly AuthenticationService _authenticationService;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthenticationController(AuthenticationService authenticationService, UserManager<ApplicationUser> userManager)
    {
        _authenticationService = authenticationService;
        _userManager = userManager;
    }

    [HttpPost]
    [Route("LogIn")]
    public async Task<IActionResult> LogIn([FromBody] LogInForm logInForm)
    {
        try
        {
            return Ok(await _authenticationService.LogIn(logInForm));
        }
        catch (LogInException)
        {
            return Unauthorized("Failed to log in");
        }
    }

    [HttpPost]
    [Route("SignUp")]
    public async Task<IActionResult> SignUp([FromBody] SignUpForm signUpForm)
    {
        if (!ModelState.IsValid || string.IsNullOrEmpty(signUpForm.ProfilePicture) || string.IsNullOrEmpty(signUpForm.FreetradeCookie))
        {
            return BadRequest("All fields are required");
        }

        try
        {
            return Ok(await _authenticationService.SignUp(signUpForm));
        }
        catch (UserAlreadyExistsException)
        {
            return BadRequest("User already exists with this discord username");
        }
    }

    [HttpPost]
    [Route("Refresh")]
    public async Task<IActionResult> Refresh()
    {
        string refreshToken = await new StreamReader(Request.Body).ReadToEndAsync();
        if (string.IsNullOrEmpty(refreshToken)) return BadRequest("Refresh token is required");

        string userId = _userManager.GetUserId(User) ?? throw new LogInException();
        return Ok(await _authenticationService.RefreshToken(refreshToken, userId));
    }
}