using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BitcoinMarketLoader.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BitcoinMarketLoader.Domain.Market;

public class MarketTick
{
    [Key]
    public long Ccseq { get; set; }

    public DateTime GeneratedTimestamp { get; set; }
    
    [StringLength(100)]
    public string? Market { get; set; }

    [StringLength(100)]
    public string? Instrument { get; set; }
    
    public MarketCurrency? BaseCurrency { get; set; }
    public MarketCurrency? QuoteCurrency { get; set; }
    public TradeDetails? LastTrade { get; set; }
    public TradeDetails? LastProcessedTrade { get; set; }

    [Precision(18,8)]
    public decimal? Price { get; set; }
    public string? PriceFlag { get; set; }
    public MarketTimestamp? PriceUpdateTimestamp { get; set; }
    
    public Order? BestBid { get; set; }
    public Order? BestAsk { get; set; }
    
    /// <summary>
    /// User-defined note
    /// </summary>
    [StringLength(1024)]
    public string? Note { get; set; }
    
    [JsonIgnore]
    public ICollection<TradeStatistics>? TradeStatistics { get; set; }
    
    [NotMapped]
    public Dictionary<TimeWindowRanges, TradeStatistics>? TradeStatisticsByRange => TradeStatistics?.ToDictionary(ts => ts.Range, ts => ts);
}
