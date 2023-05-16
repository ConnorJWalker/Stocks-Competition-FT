namespace StocksCompetition.Shared;

public class JwtResponse
{
    public string Token { get; }

    public DateTime ValidTo { get; }

    public string RefreshToken { get; }

    public JwtResponse(string token, DateTime validTo, string refreshToken)
    {
        Token = token;
        ValidTo = validTo;
        RefreshToken = refreshToken;
    }
}
