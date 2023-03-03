using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StocksCompetition.Server.Services;
using StocksCompetition.Shared;

namespace StocksCompetition.Server.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class AuthenticationController : Controller
{
    private readonly AuthenticationService _authenticationService;
    
    public AuthenticationController(AuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    [HttpPost]
    [Route("LogIn")]
    public async Task<IActionResult> LogIn([FromBody] LogInForm logInForm)
    {
        try
        {
            JwtSecurityToken token = await _authenticationService.LogIn(logInForm);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
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
            JwtSecurityToken token = await _authenticationService.SignUp(signUpForm);
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                expiration = token.ValidTo
            });
        }
        catch (UserAlreadyExistsException)
        {
            return BadRequest("User already exists with this discord username");
        }
    }
}