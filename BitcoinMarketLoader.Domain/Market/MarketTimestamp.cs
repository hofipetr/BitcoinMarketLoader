using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace BitcoinMarketLoader.Domain.Market;

public class MarketTimestamp
{
    public required long UnixSeconds { get; init; }
    public long Nanoseconds { get; init; } = 0;
    
    [NotMapped]
    public DateTime Date => DateTime.UnixEpoch.AddSeconds(UnixSeconds).AddTicks(Nanoseconds / 100);
}