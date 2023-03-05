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

public struct GainLoss
{
    public float Percentage { get; set; }

    public Value Value { get; set; }
}

public struct Value
{
    public string Currency { get; set; }

    [JsonProperty("Value")]
    public double Amount { get; set; }
}

public struct PriceHistoryValue
{
    public DateTime Time { get; set; }

    public Value Value { get; set; }

    public Value ValueChange { get; set; }

    public float PercentageChange { get; set; }
}
