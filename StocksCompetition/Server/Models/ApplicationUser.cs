using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace StocksCompetition.Server.Models;

public class ApplicationUser : IdentityUser
{
    public string ProfilePicture { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
    
    public string DiscordUsername { get; set; } = string.Empty;

    public string DisplayColour { get; set; } = string.Empty;
    
    public string Password { get; set; } = string.Empty;
    
    public string FreetradeCookie { get; set; } = string.Empty;
}