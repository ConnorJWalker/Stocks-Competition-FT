namespace StocksCompetition.Server.Models;

public class FreetradeRequestBody
{
    public string OperationName { get; set; }

    public Dictionary<string, string> Variables { get; set; }

    public string Query { get; set; }
}