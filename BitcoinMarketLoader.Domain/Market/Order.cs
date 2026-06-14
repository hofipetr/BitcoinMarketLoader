using Microsoft.EntityFrameworkCore;

namespace BitcoinMarketLoader.Domain.Market;

public class Order
{
    [Precision(18,8)]
    public decimal? Price { get; set; }
    [Precision(18,8)]
    public decimal? Quantity { get; set; }
    [Precision(18,8)]
    public decimal? QuoteQuantity { get; set; }
    public MarketTimestamp? LastUpdateTimestamp { get; set; }
    public MarketTimestamp? PositionUpdateTimestamp { get; set; }
}