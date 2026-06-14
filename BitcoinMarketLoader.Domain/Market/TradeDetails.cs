using System.ComponentModel.DataAnnotations;
using BitcoinMarketLoader.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace BitcoinMarketLoader.Domain.Market;

public class TradeDetails
{
    [Key]
    public required long Ccseq { get; set; }

    public string? TradeId { get; set; }
    
    [Precision(18,8)]
    public Decimal? Quantity { get; set; }
    [Precision(18,8)]
    public Decimal? QuoteQuantity { get; set; }
    [Precision(18,8)]
    public Decimal? Price { get; set; }
    public MarketTimestamp? Timestamp { get; set; }
    public MarketTimestamp? ReceivedTimestamp { get; set; }
    public TradeSides? Side { get; set; }
    
}