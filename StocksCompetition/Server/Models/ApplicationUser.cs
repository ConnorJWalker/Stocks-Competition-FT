using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using StocksCompetition.Shared;

namespace StocksCompetition.Server.Models;

public class ApplicationUser : IdentityUser
{
    public string ProfilePicture { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;
    
    public string DiscordUsername { get; set; } = string.Empty;

    public string DisplayColour { get; set; } = string.Empty;
    
    [NotMapped]
    public string Password { get; set; } = string.Empty;
    
    public string FreetradeCookie { get; set; } = string.Empty;
    
    public ApplicationUser() { }
    
    public ApplicationUser(SignUpForm signUpForm)
    {
        ProfilePicture = signUpForm.ProfilePicture;
        DisplayName = signUpForm.DisplayName;
        DiscordUsername = signUpForm.DiscordUsername;
        DisplayColour = signUpForm.DisplayColour;
        Password = signUpForm.Password;
        FreetradeCookie = signUpForm.FreetradeCookie;
    }
}