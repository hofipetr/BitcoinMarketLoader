namespace BitcoinMarketLoader.Models;

public sealed record PagedResponse<T>(
    IReadOnlyCollection<T> Data,
    int Page,
    int PageSize,
    int TotalCount);

public sealed class MarketTickDto
{
    public long Ccseq { get; set; }
    public DateTime GeneratedTimestamp { get; set; }
    public string? Market { get; set; }
    public string? Instrument { get; set; }
    public MarketCurrencyDto? BaseCurrency { get; set; }
    public MarketCurrencyDto? QuoteCurrency { get; set; }
    public TradeDetailsDto? LastTrade { get; set; }
    public TradeDetailsDto? LastProcessedTrade { get; set; }
    public decimal? Price { get; set; }
    public string? PriceFlag { get; set; }
    public MarketTimestampDto? PriceUpdateTimestamp { get; set; }
    public OrderDto? BestBid { get; set; }
    public OrderDto? BestAsk { get; set; }
    public string? Note { get; set; }
    public Dictionary<string, TradeStatisticsDto>? TradeStatisticsByRange { get; set; }
}

public sealed class MarketCurrencyDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
}

public sealed class TradeDetailsDto
{
    public long Ccseq { get; set; }
    public string? TradeId { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? QuoteQuantity { get; set; }
    public decimal? Price { get; set; }
    public MarketTimestampDto? Timestamp { get; set; }
    public MarketTimestampDto? ReceivedTimestamp { get; set; }
    public int? Side { get; set; }
}

public sealed class OrderDto
{
    public decimal? Price { get; set; }
    public decimal? Quantity { get; set; }
    public decimal? QuoteQuantity { get; set; }
    public MarketTimestampDto? LastUpdateTimestamp { get; set; }
    public MarketTimestampDto? PositionUpdateTimestamp { get; set; }
}

public sealed class MarketTimestampDto
{
    public long UnixSeconds { get; set; }
    public long Nanoseconds { get; set; }

    public DateTime Date =>
        DateTime.UnixEpoch.AddSeconds(UnixSeconds).AddTicks(Nanoseconds / 100);
}

public sealed class TradeStatisticsDto
{
    public int Range { get; set; }
    public long SourceTickCcseq { get; set; }
    public decimal? VolumeTotal { get; set; }
    public decimal? VolumeBuy { get; set; }
    public decimal? VolumeSell { get; set; }
    public decimal? VolumeUnknown { get; set; }
    public decimal? QuoteVolumeTotal { get; set; }
    public decimal? QuoteVolumeBuy { get; set; }
    public decimal? QuoteVolumeSell { get; set; }
    public decimal? QuoteVolumeUnknown { get; set; }
    public decimal? OpenPrice { get; set; }
    public decimal? HighPrice { get; set; }
    public decimal? LowPrice { get; set; }
    public decimal? PriceChange { get; set; }
    public decimal? PriceChangePercent { get; set; }
    public MarketTimestampDto? HighPriceTimestamp { get; set; }
    public MarketTimestampDto? LowPriceTimestamp { get; set; }
    public MarketTimestampDto? FirstTradeTimestamp { get; set; }
    public long? TotalTrades { get; set; }
    public long? TotalBuys { get; set; }
    public long? TotalSells { get; set; }
    public long? TotalUnknownTrades { get; set; }
}
