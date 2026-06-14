using BitcoinMarketLoader.Application.Enums;
using BitcoinMarketLoader.Application.Models;
using BitcoinMarketLoader.Domain.Market;

namespace BitcoinMarketLoader.Application.Services;

public interface IBitCoinDataService
{
    /// <summary>
    /// Get latest BTC spot market tick and optionally save it in DB.
    /// </summary>
    /// <param name="currencyCodes">Currency in which to return the tick. Supported values are EUR and CZK.</param>
    /// <param name="save">If true, save the tick also to DB.</param>
    /// <returns><see cref="MarketTick"/> instance on success. Null on failure</returns>
    Task<MarketTick?> FetchLatestBtcMarketTickAsync(CurrencyCodes currencyCodes, bool save = true);
    
    /// <summary>
    /// Get market tick data from database.
    /// </summary>
    /// <param name="currencyCodes">Currency in which to return the tick. Supported values are EUR and CZK.</param>
    /// <param name="marketTickId">Tick id (ccseq)</param>
    /// <returns><see cref="MarketTick"/> instance on success. Null if not found</returns>
    Task<MarketTick?> GetBtcMarketTickAsync(long marketTickId, CurrencyCodes currencyCodes);
    
    /// <summary>
    /// Get paged market tick data from database.
    /// </summary>
    /// <param name="currencyCodes">Currency in which to return the ticks. Supported values are EUR and CZK.</param>
    /// <param name="page">Page number.</param>
    /// <param name="pageSize">Number of ticks per page.</param>
    /// <returns>A page of market ticks.</returns>
    Task<PagedResponse<MarketTick>> GetBtcMarketTicksAsync(
        CurrencyCodes currencyCodes,
        int page = 1,
        int pageSize = 10);
    
    /// <summary>
    /// Update user note on a <see cref="MarketTick"/>
    /// </summary>
    /// <param name="marketTickId">Tick id (ccseq)</param>
    /// <param name="note">New note</param>
    /// <returns>True on success, false on failure</returns>
    Task<bool> UpdateNoteForMarketTickAsync(long marketTickId, string? note);
}
