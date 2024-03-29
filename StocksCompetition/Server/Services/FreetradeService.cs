using Newtonsoft.Json.Linq;
using StocksCompetition.Server.Models;
using StocksCompetition.Shared.Freetrade;

namespace StocksCompetition.Server.Services;

public class FreetradeService
{
    private readonly HttpClient _client;
    private readonly Dictionary<string, string> _queries = new();

    public FreetradeService(IConfiguration configuration)
    {
        // Cookies must be disabled in the client handler to allow us to set our own cookie header
        var handler = new HttpClientHandler { UseCookies = false };
        _client = new HttpClient(handler)
        {
            BaseAddress =  new Uri(configuration["FreetradeAddress"]
                ?? throw new MissingFieldException("No freetrade base address provided"))
        };

        string directory = Path.Combine(Directory.GetCurrentDirectory(), "Models/FreetradeQueries");
        string[] files = Directory.GetFiles(directory);
        foreach (string file in files)
        {
            _queries.Add(Path.GetFileName(file).Split(".")[0], File.ReadAllText(file));
        }
    }

    public async Task<AccountDetails> GetUserChart(string cookie)
    {
        var body = new FreetradeRequestBody()
        {
            OperationName = "AccountChart",
            Query = _queries["AccountChart"],
            Variables = new Dictionary<string, string>()
            {
                { "period", "ONEWEEK" },
                { "type", "ISA" }
            }
        };

        var message = new HttpRequestMessage(HttpMethod.Post, "");
        message.Content = JsonContent.Create(body);
        message.Headers.Add("Cookie", $"ft_web_session={cookie}");

        HttpResponseMessage result = await _client.SendAsync(message);
        string responseString = await result.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(responseString))
        {
            throw new FreetradeException("Could not load user chart from Freetrade");
        }
        
        return JObject.Parse(responseString)["data"]?["accountDetails"]?.ToObject<AccountDetails>()
            ?? throw new FreetradeException("Cannot convert response to type AccountDetails");
    }
}

public class FreetradeException : Exception
{
    public FreetradeException(string message) : base(message) { }
}
