using System.ComponentModel.DataAnnotations.Schema;

namespace BitcoinMarketLoader.Domain.Market;

/// <summary>
/// Timestamp representation used by market data. Designed according to timestamps returned from  Coindesk API (see https://developers.coindesk.com/documentation/data-api)
/// </summary>
public class MarketTimestamp
{
    public MarketTimestamp()
    {
        
    }

    public MarketTimestamp(DateTime dateTime)
    {
        var unixSpan = (dateTime - DateTime.UnixEpoch);
        UnixSeconds = (long)unixSpan.TotalSeconds;
        Nanoseconds = unixSpan.Nanoseconds;
    }
    
    /// <summary>
    /// The Current Epoch Unix Timestamp  
    /// </summary>
    public long UnixSeconds { get; init; }
    
    /// <summary>
    /// Fractional part of the timestamp for better precision  
    /// </summary>
    public long Nanoseconds { get; init; } = 0;
    
    [NotMapped]
    public DateTime Date => DateTime.UnixEpoch.AddSeconds(UnixSeconds).AddTicks(Nanoseconds / 100);
}