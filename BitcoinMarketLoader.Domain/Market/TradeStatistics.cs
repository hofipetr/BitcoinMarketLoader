using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using BitcoinMarketLoader.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BitcoinMarketLoader.Domain.Market;

public class TradeStatistics
{
    public TimeWindowRanges Range { get; set; }
    
    [ForeignKey(nameof(SourceTick))]
    public long SourceTickCcseq { get; set; }
    
    [JsonIgnore]
    public MarketTick? SourceTick { get; set; }

    [Precision(18,8)]
    public decimal? VolumeTotal { get; set; }
    [Precision(18,8)]
    public decimal? VolumeBuy { get; set; }
    [Precision(18,8)]
    public decimal? VolumeSell { get; set; }
    [Precision(18,8)]
    public decimal? VolumeUnknown { get; set; }

    [Precision(22,8)]
    public decimal? QuoteVolumeTotal { get; set; }
    [Precision(22,8)]
    public decimal? QuoteVolumeBuy { get; set; }
    [Precision(22,8)]
    public decimal? QuoteVolumeSell { get; set; }
    [Precision(22,8)]
    public decimal? QuoteVolumeUnknown { get; set; }

    [Precision(18,8)]
    public decimal? OpenPrice { get; set; }
    [Precision(18,8)]
    public decimal? HighPrice { get; set; }
    [Precision(18,8)]
    public decimal? LowPrice { get; set; }
    [Precision(18,8)]
    public decimal? PriceChange { get; set; }
    [Precision(18,8)]
    public decimal? PriceChangePercent { get; set; }

    public MarketTimestamp? HighPriceTimestamp { get; set; }
    public MarketTimestamp? LowPriceTimestamp { get; set; }
    public MarketTimestamp? FirstTradeTimestamp { get; set; }
    
    public long? TotalTrades { get; set; }
    public long? TotalBuys { get; set; }
    public long? TotalSells { get; set; }
    public long? TotalUnknownTrades { get; set; }
    
}