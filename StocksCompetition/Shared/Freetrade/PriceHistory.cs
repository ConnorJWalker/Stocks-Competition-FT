using Newtonsoft.Json;

namespace StocksCompetition.Shared.Freetrade;

public class PriceHistory
{
    public Guid Id { get; set; }

    public string Period { get; set; }

    public DateTime LastUpdated { get; set; }

    public GainLoss GainLoss { get; set; }
    
    public List<PriceHistoryValue> Data { get; set; }
}

public class GainLoss
{
    public float Percentage { get; set; }

    public Value Value { get; set; }
}

public class Value
{
    public string Currency { get; set; }

    [JsonProperty("Value")]
    public decimal Amount { get; set; }
}

public class PriceHistoryValue
{
    public DateTime Time { get; set; }

    public Value Value { get; set; }

    public Value ValueChange { get; set; }

    public float PercentageChange { get; set; }
}
