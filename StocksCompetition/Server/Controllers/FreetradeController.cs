using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StocksCompetition.Server.Models;
using StocksCompetition.Server.Services;

namespace StocksCompetition.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FreetradeController : Controller
{
    private readonly FreetradeService _freetradeService;
    private readonly UserManager<ApplicationUser> _userManager;

    public FreetradeController(FreetradeService freetradeService, UserManager<ApplicationUser> userManager)
    {
        _freetradeService = freetradeService;
        _userManager = userManager;
    }
    
    [HttpGet]
    [Route("UserChart")]
    public async Task<IActionResult> GetUserChart()
    {
        ClaimsPrincipal principal = User;
        ApplicationUser user = (await _userManager.GetUserAsync(principal))!;

        try
        {
            return Ok(await _freetradeService.GetUserChart(user.FreetradeCookie));
        }
        catch (FreetradeException e)
        {
            return StatusCode(500, e.Message);
        }
    }
}