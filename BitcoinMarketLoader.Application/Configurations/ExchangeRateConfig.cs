namespace BitcoinMarketLoader.Application.Configurations;

public class ExchangeRateConfig
{
    public const string SectionName = "ExchangeRate";
    
    public int CacheDurationInHours { get; init; } = 24 * 7;
    public int CurrentDayCacheDurationInMinutes { get; init; } = 60;
    
    public TimeSpan CacheDuration => TimeSpan.FromHours(CacheDurationInHours);
    public TimeSpan CurrentDayCacheDuration => TimeSpan.FromMinutes(CurrentDayCacheDurationInMinutes);
}

