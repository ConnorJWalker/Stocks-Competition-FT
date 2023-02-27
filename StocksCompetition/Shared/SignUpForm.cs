using System.ComponentModel.DataAnnotations;

namespace StocksCompetition.Shared;

public class SignUpForm
{
    public string ProfilePicture { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Display Name Field is Required")]
    public string DisplayName { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Discord Username Field is Required")]
    public string DiscordUsername { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Display Colour is Required")]
    public string DisplayColour { get; set; }
    
    [Required(ErrorMessage = "The Password Field is Required")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Confirm Password Field is Required")]
    [Compare("Password", ErrorMessage = "Passwords Must Match")]
    public string ConfirmPassword { get; set; } = string.Empty;

    public string FreetradeCookie { get; set; } = string.Empty;

    public SignUpForm()
    {
        char[] validCharacters = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        var random = new Random();
        
        DisplayColour = "#" + new string(validCharacters.OrderBy(c => random.Next()).Take(6).ToArray());
    }
}