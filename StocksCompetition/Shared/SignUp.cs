namespace StocksCompetition.Shared;

public class SignUp
{
    public string ProfilePicture { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string DiscordUsername { get; set; } = string.Empty;

    public string DisplayColour { get; set; }

    public string Password { get; set; } = string.Empty;

    public string ConfirmPassword { get; set; } = string.Empty;

    public string FreetradeCookie { get; set; } = string.Empty;

    public SignUp()
    {
        char[] validCharacters = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        var random = new Random();
        
        DisplayColour = "#" + new string(validCharacters.OrderBy(c => random.Next()).Take(6).ToArray());
    }
}