namespace BitcoinMarketLoader.Application.Configurations;

public sealed class MarketTickPollingOptions
{
    public const string SectionName = "MarketTickPolling";

    public int IntervalSeconds { get; init; } = 5;
}
