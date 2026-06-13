using System.Text.Json.Serialization;

namespace BitcoinMarketLoader.Application.Dtos.MarketTickDtos;

public class MarketTickDto
{
    public Dictionary<string, InstrumentDataDto>? Data { get; set; }
    
    [JsonPropertyName("Err")]
    public ErrorDto? Error { get; set; }
}

public class InstrumentDataDto
{
    // Identifiers / mapping
    [JsonPropertyName("TYPE")]
    public string? Type { get; set; }

    [JsonPropertyName("MARKET")]
    public string? Market { get; set; }

    [JsonPropertyName("INSTRUMENT")]
    public string? Instrument { get; set; }

    [JsonPropertyName("MAPPED_INSTRUMENT")]
    public string? MappedInstrument { get; set; }

    [JsonPropertyName("BASE")]
    public string? Base { get; set; }

    [JsonPropertyName("QUOTE")]
    public string? Quote { get; set; }

    [JsonPropertyName("BASE_ID")]
    public long? BaseId { get; set; }

    [JsonPropertyName("QUOTE_ID")]
    public long? QuoteId { get; set; }

    [JsonPropertyName("TRANSFORM_FUNCTION")]
    public string? TransformFunction { get; set; }

    // Core values
    [JsonPropertyName("CCSEQ")]
    public long? Ccseq { get; set; }

    [JsonPropertyName("PRICE")]
    public decimal? Price { get; set; }

    [JsonPropertyName("PRICE_FLAG")]
    public string? PriceFlag { get; set; }

    [JsonPropertyName("PRICE_LAST_UPDATE_TS")]
    public long? PriceLastUpdateTs { get; set; }

    [JsonPropertyName("PRICE_LAST_UPDATE_TS_NS")]
    public long? PriceLastUpdateTsNs { get; set; }

    // Last trade
    [JsonPropertyName("LAST_TRADE_QUANTITY")]
    public decimal? LastTradeQuantity { get; set; }

    [JsonPropertyName("LAST_TRADE_QUOTE_QUANTITY")]
    public decimal? LastTradeQuoteQuantity { get; set; }

    [JsonPropertyName("LAST_TRADE_ID")]
    public string? LastTradeId { get; set; }

    [JsonPropertyName("LAST_TRADE_CCSEQ")]
    public long? LastTradeCcseq { get; set; }

    [JsonPropertyName("LAST_TRADE_SIDE")]
    public string? LastTradeSide { get; set; }

    // Last received / processed timestamps
    [JsonPropertyName("LAST_TRADE_RECEIVED_TS")]
    public long? LastTradeReceivedTs { get; set; }

    [JsonPropertyName("LAST_TRADE_RECEIVED_TS_NS")]
    public long? LastTradeReceivedTsNs { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_TS")]
    public long? LastProcessedTradeTs { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_TS_NS")]
    public long? LastProcessedTradeTsNs { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_RECEIVED_TS")]
    public long? LastProcessedTradeReceivedTs { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_RECEIVED_TS_NS")]
    public long? LastProcessedTradeReceivedTsNs { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_PRICE")]
    public decimal? LastProcessedTradePrice { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_QUANTITY")]
    public decimal? LastProcessedTradeQuantity { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_QUOTE_QUANTITY")]
    public decimal? LastProcessedTradeQuoteQuantity { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_SIDE")]
    public string? LastProcessedTradeSide { get; set; }

    [JsonPropertyName("LAST_PROCESSED_TRADE_CCSEQ")]
    public long? LastProcessedTradeCcseq { get; set; }

    // Top of book
    [JsonPropertyName("BEST_BID")]
    public decimal? BestBid { get; set; }

    [JsonPropertyName("BEST_BID_QUANTITY")]
    public decimal? BestBidQuantity { get; set; }

    [JsonPropertyName("BEST_BID_QUOTE_QUANTITY")]
    public decimal? BestBidQuoteQuantity { get; set; }

    [JsonPropertyName("BEST_BID_LAST_UPDATE_TS")]
    public long? BestBidLastUpdateTs { get; set; }

    [JsonPropertyName("BEST_BID_LAST_UPDATE_TS_NS")]
    public long? BestBidLastUpdateTsNs { get; set; }

    [JsonPropertyName("BEST_BID_POSITION_IN_BOOK_UPDATE_TS")]
    public long? BestBidPositionInBookUpdateTs { get; set; }

    [JsonPropertyName("BEST_BID_POSITION_IN_BOOK_UPDATE_TS_NS")]
    public long? BestBidPositionInBookUpdateTsNs { get; set; }

    [JsonPropertyName("BEST_ASK")]
    public decimal? BestAsk { get; set; }

    [JsonPropertyName("BEST_ASK_QUANTITY")]
    public decimal? BestAskQuantity { get; set; }

    [JsonPropertyName("BEST_ASK_QUOTE_QUANTITY")]
    public decimal? BestAskQuoteQuantity { get; set; }

    [JsonPropertyName("BEST_ASK_LAST_UPDATE_TS")]
    public long? BestAskLastUpdateTs { get; set; }

    [JsonPropertyName("BEST_ASK_LAST_UPDATE_TS_NS")]
    public long? BestAskLastUpdateTsNs { get; set; }

    [JsonPropertyName("BEST_ASK_POSITION_IN_BOOK_UPDATE_TS")]
    public long? BestAskPositionInBookUpdateTs { get; set; }

    [JsonPropertyName("BEST_ASK_POSITION_IN_BOOK_UPDATE_TS_NS")]
    public long? BestAskPositionInBookUpdateTsNs { get; set; }

    // Current hour stats
    [JsonPropertyName("CURRENT_HOUR_VOLUME")]
    public decimal? CurrentHourVolume { get; set; }

    [JsonPropertyName("CURRENT_HOUR_VOLUME_BUY")]
    public decimal? CurrentHourVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_HOUR_VOLUME_SELL")]
    public decimal? CurrentHourVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_HOUR_VOLUME_UNKNOWN")]
    public decimal? CurrentHourVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_HOUR_QUOTE_VOLUME")]
    public decimal? CurrentHourQuoteVolume { get; set; }

    [JsonPropertyName("CURRENT_HOUR_QUOTE_VOLUME_BUY")]
    public decimal? CurrentHourQuoteVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_HOUR_QUOTE_VOLUME_SELL")]
    public decimal? CurrentHourQuoteVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_HOUR_QUOTE_VOLUME_UNKNOWN")]
    public decimal? CurrentHourQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_HOUR_OPEN")]
    public decimal? CurrentHourOpen { get; set; }

    [JsonPropertyName("CURRENT_HOUR_HIGH")]
    public decimal? CurrentHourHigh { get; set; }

    [JsonPropertyName("CURRENT_HOUR_LOW")]
    public decimal? CurrentHourLow { get; set; }

    [JsonPropertyName("CURRENT_HOUR_TOTAL_TRADES")]
    public long? CurrentHourTotalTrades { get; set; }

    [JsonPropertyName("CURRENT_HOUR_TOTAL_TRADES_BUY")]
    public long? CurrentHourTotalTradesBuy { get; set; }

    [JsonPropertyName("CURRENT_HOUR_TOTAL_TRADES_SELL")]
    public long? CurrentHourTotalTradesSell { get; set; }

    [JsonPropertyName("CURRENT_HOUR_TOTAL_TRADES_UNKNOWN")]
    public long? CurrentHourTotalTradesUnknown { get; set; }

    [JsonPropertyName("CURRENT_HOUR_ASSET_VOLUME_USD")]
    public decimal? CurrentHourAssetVolumeUsd { get; set; }

    [JsonPropertyName("CURRENT_HOUR_CHANGE")]
    public decimal? CurrentHourChange { get; set; }

    [JsonPropertyName("CURRENT_HOUR_CHANGE_PERCENTAGE")]
    public decimal? CurrentHourChangePercentage { get; set; }

    // Current day
    [JsonPropertyName("CURRENT_DAY_VOLUME")]
    public decimal? CurrentDayVolume { get; set; }

    [JsonPropertyName("CURRENT_DAY_VOLUME_BUY")]
    public decimal? CurrentDayVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_DAY_VOLUME_SELL")]
    public decimal? CurrentDayVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_DAY_VOLUME_UNKNOWN")]
    public decimal? CurrentDayVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_DAY_QUOTE_VOLUME")]
    public decimal? CurrentDayQuoteVolume { get; set; }

    [JsonPropertyName("CURRENT_DAY_QUOTE_VOLUME_BUY")]
    public decimal? CurrentDayQuoteVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_DAY_QUOTE_VOLUME_SELL")]
    public decimal? CurrentDayQuoteVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_DAY_QUOTE_VOLUME_UNKNOWN")]
    public decimal? CurrentDayQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_DAY_OPEN")]
    public decimal? CurrentDayOpen { get; set; }

    [JsonPropertyName("CURRENT_DAY_HIGH")]
    public decimal? CurrentDayHigh { get; set; }

    [JsonPropertyName("CURRENT_DAY_LOW")]
    public decimal? CurrentDayLow { get; set; }

    [JsonPropertyName("CURRENT_DAY_TOTAL_TRADES")]
    public long? CurrentDayTotalTrades { get; set; }

    [JsonPropertyName("CURRENT_DAY_TOTAL_TRADES_BUY")]
    public long? CurrentDayTotalTradesBuy { get; set; }

    [JsonPropertyName("CURRENT_DAY_TOTAL_TRADES_SELL")]
    public long? CurrentDayTotalTradesSell { get; set; }

    [JsonPropertyName("CURRENT_DAY_TOTAL_TRADES_UNKNOWN")]
    public long? CurrentDayTotalTradesUnknown { get; set; }

    [JsonPropertyName("CURRENT_DAY_ASSET_VOLUME_USD")]
    public decimal? CurrentDayAssetVolumeUsd { get; set; }

    [JsonPropertyName("CURRENT_DAY_CHANGE")]
    public decimal? CurrentDayChange { get; set; }

    [JsonPropertyName("CURRENT_DAY_CHANGE_PERCENTAGE")]
    public decimal? CurrentDayChangePercentage { get; set; }

    // Current week
    [JsonPropertyName("CURRENT_WEEK_VOLUME")]
    public decimal? CurrentWeekVolume { get; set; }

    [JsonPropertyName("CURRENT_WEEK_VOLUME_BUY")]
    public decimal? CurrentWeekVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_WEEK_VOLUME_SELL")]
    public decimal? CurrentWeekVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_WEEK_VOLUME_UNKNOWN")]
    public decimal? CurrentWeekVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_WEEK_QUOTE_VOLUME")]
    public decimal? CurrentWeekQuoteVolume { get; set; }

    [JsonPropertyName("CURRENT_WEEK_QUOTE_VOLUME_BUY")]
    public decimal? CurrentWeekQuoteVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_WEEK_QUOTE_VOLUME_SELL")]
    public decimal? CurrentWeekQuoteVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_WEEK_QUOTE_VOLUME_UNKNOWN")]
    public decimal? CurrentWeekQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_WEEK_OPEN")]
    public decimal? CurrentWeekOpen { get; set; }

    [JsonPropertyName("CURRENT_WEEK_HIGH")]
    public decimal? CurrentWeekHigh { get; set; }

    [JsonPropertyName("CURRENT_WEEK_LOW")]
    public decimal? CurrentWeekLow { get; set; }

    [JsonPropertyName("CURRENT_WEEK_TOTAL_TRADES")]
    public long? CurrentWeekTotalTrades { get; set; }

    [JsonPropertyName("CURRENT_WEEK_TOTAL_TRADES_BUY")]
    public long? CurrentWeekTotalTradesBuy { get; set; }

    [JsonPropertyName("CURRENT_WEEK_TOTAL_TRADES_SELL")]
    public long? CurrentWeekTotalTradesSell { get; set; }

    [JsonPropertyName("CURRENT_WEEK_TOTAL_TRADES_UNKNOWN")]
    public long? CurrentWeekTotalTradesUnknown { get; set; }

    [JsonPropertyName("CURRENT_WEEK_ASSET_VOLUME_USD")]
    public decimal? CurrentWeekAssetVolumeUsd { get; set; }

    [JsonPropertyName("CURRENT_WEEK_CHANGE")]
    public decimal? CurrentWeekChange { get; set; }

    [JsonPropertyName("CURRENT_WEEK_CHANGE_PERCENTAGE")]
    public decimal? CurrentWeekChangePercentage { get; set; }

    // Current month
    [JsonPropertyName("CURRENT_MONTH_VOLUME")]
    public decimal? CurrentMonthVolume { get; set; }

    [JsonPropertyName("CURRENT_MONTH_VOLUME_BUY")]
    public decimal? CurrentMonthVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_MONTH_VOLUME_SELL")]
    public decimal? CurrentMonthVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_MONTH_VOLUME_UNKNOWN")]
    public decimal? CurrentMonthVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_MONTH_QUOTE_VOLUME")]
    public decimal? CurrentMonthQuoteVolume { get; set; }

    [JsonPropertyName("CURRENT_MONTH_QUOTE_VOLUME_BUY")]
    public decimal? CurrentMonthQuoteVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_MONTH_QUOTE_VOLUME_SELL")]
    public decimal? CurrentMonthQuoteVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_MONTH_QUOTE_VOLUME_UNKNOWN")]
    public decimal? CurrentMonthQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_MONTH_OPEN")]
    public decimal? CurrentMonthOpen { get; set; }

    [JsonPropertyName("CURRENT_MONTH_HIGH")]
    public decimal? CurrentMonthHigh { get; set; }

    [JsonPropertyName("CURRENT_MONTH_LOW")]
    public decimal? CurrentMonthLow { get; set; }

    [JsonPropertyName("CURRENT_MONTH_TOTAL_TRADES")]
    public long? CurrentMonthTotalTrades { get; set; }

    [JsonPropertyName("CURRENT_MONTH_TOTAL_TRADES_BUY")]
    public long? CurrentMonthTotalTradesBuy { get; set; }

    [JsonPropertyName("CURRENT_MONTH_TOTAL_TRADES_SELL")]
    public long? CurrentMonthTotalTradesSell { get; set; }

    [JsonPropertyName("CURRENT_MONTH_TOTAL_TRADES_UNKNOWN")]
    public long? CurrentMonthTotalTradesUnknown { get; set; }

    [JsonPropertyName("CURRENT_MONTH_ASSET_VOLUME_USD")]
    public decimal? CurrentMonthAssetVolumeUsd { get; set; }

    [JsonPropertyName("CURRENT_MONTH_CHANGE")]
    public decimal? CurrentMonthChange { get; set; }

    [JsonPropertyName("CURRENT_MONTH_CHANGE_PERCENTAGE")]
    public decimal? CurrentMonthChangePercentage { get; set; }

    // Current year
    [JsonPropertyName("CURRENT_YEAR_VOLUME")]
    public decimal? CurrentYearVolume { get; set; }

    [JsonPropertyName("CURRENT_YEAR_VOLUME_BUY")]
    public decimal? CurrentYearVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_YEAR_VOLUME_SELL")]
    public decimal? CurrentYearVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_YEAR_VOLUME_UNKNOWN")]
    public decimal? CurrentYearVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_YEAR_QUOTE_VOLUME")]
    public decimal? CurrentYearQuoteVolume { get; set; }

    [JsonPropertyName("CURRENT_YEAR_QUOTE_VOLUME_BUY")]
    public decimal? CurrentYearQuoteVolumeBuy { get; set; }

    [JsonPropertyName("CURRENT_YEAR_QUOTE_VOLUME_SELL")]
    public decimal? CurrentYearQuoteVolumeSell { get; set; }

    [JsonPropertyName("CURRENT_YEAR_QUOTE_VOLUME_UNKNOWN")]
    public decimal? CurrentYearQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("CURRENT_YEAR_OPEN")]
    public decimal? CurrentYearOpen { get; set; }

    [JsonPropertyName("CURRENT_YEAR_HIGH")]
    public decimal? CurrentYearHigh { get; set; }

    [JsonPropertyName("CURRENT_YEAR_LOW")]
    public decimal? CurrentYearLow { get; set; }

    [JsonPropertyName("CURRENT_YEAR_TOTAL_TRADES")]
    public long? CurrentYearTotalTrades { get; set; }

    [JsonPropertyName("CURRENT_YEAR_TOTAL_TRADES_BUY")]
    public long? CurrentYearTotalTradesBuy { get; set; }

    [JsonPropertyName("CURRENT_YEAR_TOTAL_TRADES_SELL")]
    public long? CurrentYearTotalTradesSell { get; set; }

    [JsonPropertyName("CURRENT_YEAR_TOTAL_TRADES_UNKNOWN")]
    public long? CurrentYearTotalTradesUnknown { get; set; }

    [JsonPropertyName("CURRENT_YEAR_ASSET_VOLUME_USD")]
    public decimal? CurrentYearAssetVolumeUsd { get; set; }

    [JsonPropertyName("CURRENT_YEAR_CHANGE")]
    public decimal? CurrentYearChange { get; set; }

    [JsonPropertyName("CURRENT_YEAR_CHANGE_PERCENTAGE")]
    public decimal? CurrentYearChangePercentage { get; set; }

    // Moving windows (24h, 7d, 30d, 90d, 180d, 365d etc)
    [JsonPropertyName("MOVING_24_HOUR_VOLUME")]
    public decimal? Moving24HourVolume { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_VOLUME_BUY")]
    public decimal? Moving24HourVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_VOLUME_SELL")]
    public decimal? Moving24HourVolumeSell { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_VOLUME_UNKNOWN")]
    public decimal? Moving24HourVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_QUOTE_VOLUME")]
    public decimal? Moving24HourQuoteVolume { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_QUOTE_VOLUME_BUY")]
    public decimal? Moving24HourQuoteVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_QUOTE_VOLUME_SELL")]
    public decimal? Moving24HourQuoteVolumeSell { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_QUOTE_VOLUME_UNKNOWN")]
    public decimal? Moving24HourQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_OPEN")]
    public decimal? Moving24HourOpen { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_HIGH")]
    public decimal? Moving24HourHigh { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_LOW")]
    public decimal? Moving24HourLow { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_TOTAL_TRADES")]
    public long? Moving24HourTotalTrades { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_TOTAL_TRADES_BUY")]
    public long? Moving24HourTotalTradesBuy { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_TOTAL_TRADES_SELL")]
    public long? Moving24HourTotalTradesSell { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_TOTAL_TRADES_UNKNOWN")]
    public long? Moving24HourTotalTradesUnknown { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_ASSET_VOLUME_USD")]
    public decimal? Moving24HourAssetVolumeUsd { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_CHANGE")]
    public decimal? Moving24HourChange { get; set; }

    [JsonPropertyName("MOVING_24_HOUR_CHANGE_PERCENTAGE")]
    public decimal? Moving24HourChangePercentage { get; set; }

    [JsonPropertyName("MOVING_7_DAY_VOLUME")]
    public decimal? Moving7DayVolume { get; set; }

    [JsonPropertyName("MOVING_7_DAY_VOLUME_BUY")]
    public decimal? Moving7DayVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_7_DAY_VOLUME_SELL")]
    public decimal? Moving7DayVolumeSell { get; set; }

    [JsonPropertyName("MOVING_7_DAY_VOLUME_UNKNOWN")]
    public decimal? Moving7DayVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_7_DAY_QUOTE_VOLUME")]
    public decimal? Moving7DayQuoteVolume { get; set; }

    [JsonPropertyName("MOVING_7_DAY_QUOTE_VOLUME_BUY")]
    public decimal? Moving7DayQuoteVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_7_DAY_QUOTE_VOLUME_SELL")]
    public decimal? Moving7DayQuoteVolumeSell { get; set; }

    [JsonPropertyName("MOVING_7_DAY_QUOTE_VOLUME_UNKNOWN")]
    public decimal? Moving7DayQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_7_DAY_OPEN")]
    public decimal? Moving7DayOpen { get; set; }

    [JsonPropertyName("MOVING_7_DAY_HIGH")]
    public decimal? Moving7DayHigh { get; set; }

    [JsonPropertyName("MOVING_7_DAY_LOW")]
    public decimal? Moving7DayLow { get; set; }

    [JsonPropertyName("MOVING_7_DAY_TOTAL_TRADES")]
    public long? Moving7DayTotalTrades { get; set; }

    [JsonPropertyName("MOVING_7_DAY_TOTAL_TRADES_BUY")]
    public long? Moving7DayTotalTradesBuy { get; set; }

    [JsonPropertyName("MOVING_7_DAY_TOTAL_TRADES_SELL")]
    public long? Moving7DayTotalTradesSell { get; set; }

    [JsonPropertyName("MOVING_7_DAY_TOTAL_TRADES_UNKNOWN")]
    public long? Moving7DayTotalTradesUnknown { get; set; }

    [JsonPropertyName("MOVING_7_DAY_ASSET_VOLUME_USD")]
    public decimal? Moving7DayAssetVolumeUsd { get; set; }

    [JsonPropertyName("MOVING_7_DAY_CHANGE")]
    public decimal? Moving7DayChange { get; set; }

    [JsonPropertyName("MOVING_7_DAY_CHANGE_PERCENTAGE")]
    public decimal? Moving7DayChangePercentage { get; set; }

    [JsonPropertyName("MOVING_30_DAY_VOLUME")]
    public decimal? Moving30DayVolume { get; set; }

    [JsonPropertyName("MOVING_30_DAY_VOLUME_BUY")]
    public decimal? Moving30DayVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_30_DAY_VOLUME_SELL")]
    public decimal? Moving30DayVolumeSell { get; set; }

    [JsonPropertyName("MOVING_30_DAY_VOLUME_UNKNOWN")]
    public decimal? Moving30DayVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_30_DAY_QUOTE_VOLUME")]
    public decimal? Moving30DayQuoteVolume { get; set; }

    [JsonPropertyName("MOVING_30_DAY_QUOTE_VOLUME_BUY")]
    public decimal? Moving30DayQuoteVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_30_DAY_QUOTE_VOLUME_SELL")]
    public decimal? Moving30DayQuoteVolumeSell { get; set; }

    [JsonPropertyName("MOVING_30_DAY_QUOTE_VOLUME_UNKNOWN")]
    public decimal? Moving30DayQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_30_DAY_OPEN")]
    public decimal? Moving30DayOpen { get; set; }

    [JsonPropertyName("MOVING_30_DAY_HIGH")]
    public decimal? Moving30DayHigh { get; set; }

    [JsonPropertyName("MOVING_30_DAY_LOW")]
    public decimal? Moving30DayLow { get; set; }

    [JsonPropertyName("MOVING_30_DAY_TOTAL_TRADES")]
    public long? Moving30DayTotalTrades { get; set; }

    [JsonPropertyName("MOVING_30_DAY_TOTAL_TRADES_BUY")]
    public long? Moving30DayTotalTradesBuy { get; set; }

    [JsonPropertyName("MOVING_30_DAY_TOTAL_TRADES_SELL")]
    public long? Moving30DayTotalTradesSell { get; set; }

    [JsonPropertyName("MOVING_30_DAY_TOTAL_TRADES_UNKNOWN")]
    public long? Moving30DayTotalTradesUnknown { get; set; }

    [JsonPropertyName("MOVING_30_DAY_ASSET_VOLUME_USD")]
    public decimal? Moving30DayAssetVolumeUsd { get; set; }

    [JsonPropertyName("MOVING_30_DAY_CHANGE")]
    public decimal? Moving30DayChange { get; set; }

    [JsonPropertyName("MOVING_30_DAY_CHANGE_PERCENTAGE")]
    public decimal? Moving30DayChangePercentage { get; set; }

    [JsonPropertyName("MOVING_90_DAY_VOLUME")]
    public decimal? Moving90DayVolume { get; set; }

    [JsonPropertyName("MOVING_90_DAY_VOLUME_BUY")]
    public decimal? Moving90DayVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_90_DAY_VOLUME_SELL")]
    public decimal? Moving90DayVolumeSell { get; set; }

    [JsonPropertyName("MOVING_90_DAY_VOLUME_UNKNOWN")]
    public decimal? Moving90DayVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_90_DAY_QUOTE_VOLUME")]
    public decimal? Moving90DayQuoteVolume { get; set; }

    [JsonPropertyName("MOVING_90_DAY_QUOTE_VOLUME_BUY")]
    public decimal? Moving90DayQuoteVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_90_DAY_QUOTE_VOLUME_SELL")]
    public decimal? Moving90DayQuoteVolumeSell { get; set; }

    [JsonPropertyName("MOVING_90_DAY_QUOTE_VOLUME_UNKNOWN")]
    public decimal? Moving90DayQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_90_DAY_OPEN")]
    public decimal? Moving90DayOpen { get; set; }

    [JsonPropertyName("MOVING_90_DAY_HIGH")]
    public decimal? Moving90DayHigh { get; set; }

    [JsonPropertyName("MOVING_90_DAY_LOW")]
    public decimal? Moving90DayLow { get; set; }

    [JsonPropertyName("MOVING_90_DAY_TOTAL_TRADES")]
    public long? Moving90DayTotalTrades { get; set; }

    [JsonPropertyName("MOVING_90_DAY_TOTAL_TRADES_BUY")]
    public long? Moving90DayTotalTradesBuy { get; set; }

    [JsonPropertyName("MOVING_90_DAY_TOTAL_TRADES_SELL")]
    public long? Moving90DayTotalTradesSell { get; set; }

    [JsonPropertyName("MOVING_90_DAY_TOTAL_TRADES_UNKNOWN")]
    public long? Moving90DayTotalTradesUnknown { get; set; }

    [JsonPropertyName("MOVING_90_DAY_ASSET_VOLUME_USD")]
    public decimal? Moving90DayAssetVolumeUsd { get; set; }

    [JsonPropertyName("MOVING_90_DAY_CHANGE")]
    public decimal? Moving90DayChange { get; set; }

    [JsonPropertyName("MOVING_90_DAY_CHANGE_PERCENTAGE")]
    public decimal? Moving90DayChangePercentage { get; set; }

    [JsonPropertyName("MOVING_180_DAY_VOLUME")]
    public decimal? Moving180DayVolume { get; set; }

    [JsonPropertyName("MOVING_180_DAY_VOLUME_BUY")]
    public decimal? Moving180DayVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_180_DAY_VOLUME_SELL")]
    public decimal? Moving180DayVolumeSell { get; set; }

    [JsonPropertyName("MOVING_180_DAY_VOLUME_UNKNOWN")]
    public decimal? Moving180DayVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_180_DAY_QUOTE_VOLUME")]
    public decimal? Moving180DayQuoteVolume { get; set; }

    [JsonPropertyName("MOVING_180_DAY_QUOTE_VOLUME_BUY")]
    public decimal? Moving180DayQuoteVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_180_DAY_QUOTE_VOLUME_SELL")]
    public decimal? Moving180DayQuoteVolumeSell { get; set; }

    [JsonPropertyName("MOVING_180_DAY_QUOTE_VOLUME_UNKNOWN")]
    public decimal? Moving180DayQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_180_DAY_OPEN")]
    public decimal? Moving180DayOpen { get; set; }

    [JsonPropertyName("MOVING_180_DAY_HIGH")]
    public decimal? Moving180DayHigh { get; set; }

    [JsonPropertyName("MOVING_180_DAY_LOW")]
    public decimal? Moving180DayLow { get; set; }

    [JsonPropertyName("MOVING_180_DAY_TOTAL_TRADES")]
    public long? Moving180DayTotalTrades { get; set; }

    [JsonPropertyName("MOVING_180_DAY_TOTAL_TRADES_BUY")]
    public long? Moving180DayTotalTradesBuy { get; set; }

    [JsonPropertyName("MOVING_180_DAY_TOTAL_TRADES_SELL")]
    public long? Moving180DayTotalTradesSell { get; set; }

    [JsonPropertyName("MOVING_180_DAY_TOTAL_TRADES_UNKNOWN")]
    public long? Moving180DayTotalTradesUnknown { get; set; }

    [JsonPropertyName("MOVING_180_DAY_ASSET_VOLUME_USD")]
    public decimal? Moving180DayAssetVolumeUsd { get; set; }

    [JsonPropertyName("MOVING_180_DAY_CHANGE")]
    public decimal? Moving180DayChange { get; set; }

    [JsonPropertyName("MOVING_180_DAY_CHANGE_PERCENTAGE")]
    public decimal? Moving180DayChangePercentage { get; set; }

    [JsonPropertyName("MOVING_365_DAY_VOLUME")]
    public decimal? Moving365DayVolume { get; set; }

    [JsonPropertyName("MOVING_365_DAY_VOLUME_BUY")]
    public decimal? Moving365DayVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_365_DAY_VOLUME_SELL")]
    public decimal? Moving365DayVolumeSell { get; set; }

    [JsonPropertyName("MOVING_365_DAY_VOLUME_UNKNOWN")]
    public decimal? Moving365DayVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_365_DAY_QUOTE_VOLUME")]
    public decimal? Moving365DayQuoteVolume { get; set; }

    [JsonPropertyName("MOVING_365_DAY_QUOTE_VOLUME_BUY")]
    public decimal? Moving365DayQuoteVolumeBuy { get; set; }

    [JsonPropertyName("MOVING_365_DAY_QUOTE_VOLUME_SELL")]
    public decimal? Moving365DayQuoteVolumeSell { get; set; }

    [JsonPropertyName("MOVING_365_DAY_QUOTE_VOLUME_UNKNOWN")]
    public decimal? Moving365DayQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("MOVING_365_DAY_OPEN")]
    public decimal? Moving365DayOpen { get; set; }

    [JsonPropertyName("MOVING_365_DAY_HIGH")]
    public decimal? Moving365DayHigh { get; set; }

    [JsonPropertyName("MOVING_365_DAY_LOW")]
    public decimal? Moving365DayLow { get; set; }

    [JsonPropertyName("MOVING_365_DAY_TOTAL_TRADES")]
    public long? Moving365DayTotalTrades { get; set; }

    [JsonPropertyName("MOVING_365_DAY_TOTAL_TRADES_BUY")]
    public long? Moving365DayTotalTradesBuy { get; set; }

    [JsonPropertyName("MOVING_365_DAY_TOTAL_TRADES_SELL")]
    public long? Moving365DayTotalTradesSell { get; set; }

    [JsonPropertyName("MOVING_365_DAY_TOTAL_TRADES_UNKNOWN")]
    public long? Moving365DayTotalTradesUnknown { get; set; }

    [JsonPropertyName("MOVING_365_DAY_ASSET_VOLUME_USD")]
    public decimal? Moving365DayAssetVolumeUsd { get; set; }

    [JsonPropertyName("MOVING_365_DAY_CHANGE")]
    public decimal? Moving365DayChange { get; set; }

    [JsonPropertyName("MOVING_365_DAY_CHANGE_PERCENTAGE")]
    public decimal? Moving365DayChangePercentage { get; set; }

    // Lifetime
    [JsonPropertyName("LIFETIME_FIRST_TRADE_TS")]
    public long? LifetimeFirstTradeTs { get; set; }

    [JsonPropertyName("LIFETIME_VOLUME")]
    public decimal? LifetimeVolume { get; set; }

    [JsonPropertyName("LIFETIME_VOLUME_BUY")]
    public decimal? LifetimeVolumeBuy { get; set; }

    [JsonPropertyName("LIFETIME_VOLUME_SELL")]
    public decimal? LifetimeVolumeSell { get; set; }

    [JsonPropertyName("LIFETIME_VOLUME_UNKNOWN")]
    public decimal? LifetimeVolumeUnknown { get; set; }

    [JsonPropertyName("LIFETIME_QUOTE_VOLUME")]
    public decimal? LifetimeQuoteVolume { get; set; }

    [JsonPropertyName("LIFETIME_QUOTE_VOLUME_BUY")]
    public decimal? LifetimeQuoteVolumeBuy { get; set; }

    [JsonPropertyName("LIFETIME_QUOTE_VOLUME_SELL")]
    public decimal? LifetimeQuoteVolumeSell { get; set; }

    [JsonPropertyName("LIFETIME_QUOTE_VOLUME_UNKNOWN")]
    public decimal? LifetimeQuoteVolumeUnknown { get; set; }

    [JsonPropertyName("LIFETIME_OPEN")]
    public decimal? LifetimeOpen { get; set; }

    [JsonPropertyName("LIFETIME_HIGH")]
    public decimal? LifetimeHigh { get; set; }

    [JsonPropertyName("LIFETIME_HIGH_TS")]
    public long? LifetimeHighTs { get; set; }

    [JsonPropertyName("LIFETIME_LOW")]
    public decimal? LifetimeLow { get; set; }

    [JsonPropertyName("LIFETIME_LOW_TS")]
    public long? LifetimeLowTs { get; set; }

    [JsonPropertyName("LIFETIME_TOTAL_TRADES")]
    public long? LifetimeTotalTrades { get; set; }

    [JsonPropertyName("LIFETIME_TOTAL_TRADES_BUY")]
    public long? LifetimeTotalTradesBuy { get; set; }

    [JsonPropertyName("LIFETIME_TOTAL_TRADES_SELL")]
    public long? LifetimeTotalTradesSell { get; set; }

    [JsonPropertyName("LIFETIME_TOTAL_TRADES_UNKNOWN")]
    public long? LifetimeTotalTradesUnknown { get; set; }

    [JsonPropertyName("LIFETIME_ASSET_VOLUME_USD")]
    public decimal? LifetimeAssetVolumeUsd { get; set; }

    [JsonPropertyName("LIFETIME_CHANGE")]
    public decimal? LifetimeChange { get; set; }

    [JsonPropertyName("LIFETIME_CHANGE_PERCENTAGE")]
    public decimal? LifetimeChangePercentage { get; set; }
}

public class ErrorDto
{
    [JsonPropertyName("type")]
    public int ErorrType { get; set; }

    [JsonPropertyName("message")]
    public string? Message { get; set; }

    [JsonPropertyName("other_info")]
    public OtherErrorInfoDto? OtherInfo { get; set; }
}

public class OtherErrorInfoDto
{
    [JsonPropertyName("properties")]
    public ICollection<PropertyErrorDto>? Properties { get; set; }
}

public class PropertyErrorDto
{
    /// <summary>
    /// The parameter that is responsible for the error (e.g. "market")
    /// </summary>
    [JsonPropertyName("param")]
    public string? Parameter { get; set; }

    /// <summary>
    /// The values responsible for the error (e.g. "test_market_does_not_exist")
    /// </summary>
    [JsonPropertyName("values")]
    public ICollection<string>? Values { get; set; }
}
