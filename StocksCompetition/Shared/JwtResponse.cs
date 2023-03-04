namespace StocksCompetition.Shared;

public class JwtResponse
{
    public string Token { get; }

    public DateTime ValidTo { get; }

    public JwtResponse(string token, DateTime validTo)
    {
        Token = token;
        ValidTo = validTo;
    }
}