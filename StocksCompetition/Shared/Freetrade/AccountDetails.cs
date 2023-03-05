namespace StocksCompetition.Shared.Freetrade;

public class AccountDetails
{
    public Guid Id { get; set; }

    public string Type { get; set; }

    public string Currency { get; set; }

    public PriceHistory History { get; set; }
}