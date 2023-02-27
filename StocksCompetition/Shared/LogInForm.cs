using System.ComponentModel.DataAnnotations;

namespace StocksCompetition.Shared;

public class LogInForm
{
    [Required(ErrorMessage = "The Discord Username Field is Required")]
    public string DiscordUsername { get; set; } = string.Empty;

    [Required(ErrorMessage = "The Discord Username Field is Required")]
    public string Password { get; set; } = string.Empty;
}