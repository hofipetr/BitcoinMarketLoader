using BitcoinMarketLoader.Domain.Market;

namespace BitcoinMarketLoader.Infrastructure.Databases;

public interface IMarketRepository
{
    /// <summary>
    /// Get total count of stored <see cref="MarketTick"/>s in the database.
    /// </summary>
    Task<int> GetMarketTicksCount();

    /// <summary>
    /// Get all <see cref="MarketTick"/>s from the database.
    /// </summary>
    /// <param name="includeDetails">If true, include all possible details. If false, include only direct properties.</param>
    /// <returns>Collection of <see cref="MarketTick"/></returns>
    Task<ICollection<MarketTick>> GetAllMarketTicks(bool includeDetails = false);
    
    /// <summary>
    /// Get paged <see cref="MarketTick"/>s from the database.
    /// </summary>
    /// <param name="skip">The page number to retrieve</param>
    /// <param name="take">The number of items per page</param>
    /// <param name="includeDetails">If true, include all possible details. If false, include only direct properties.</param>
    /// <returns>Collection of <see cref="MarketTick"/></returns>
    Task<ICollection<MarketTick>> GetMarketTicks(int skip, int take, bool includeDetails = false);
    
    /// <summary>
    /// Get a specific <see cref="MarketTick"/> from the database.
    /// </summary>
    /// <param name="ccseq">The sequence number of the market tick to retrieve</param>
    /// <param name="includeDetails">If true, include all possible details. If false, include only direct properties.</param>
    /// <returns>The requested <see cref="MarketTick"/>, or null if not found</returns>
    Task<MarketTick?> GetMarketTick(long ccseq, bool includeDetails = false);
    
    /// <summary>
    /// Insert a new <see cref="MarketTick"/> record
    /// </summary>
    /// <param name="marketTick"><see cref="MarketTick"/> record to insert</param>
    /// <returns>True if the record was inserted successfully, false otherwise</returns>
    Task<bool> AddMarketTick(MarketTick marketTick);

    /// <summary>
    /// Update user note on a <see cref="MarketTick"/>
    /// </summary>
    /// <param name="ccseq">The sequence number of the market tick to update</param>
    /// <param name="note">The new note to set</param>
    /// <returns>True if the record was updated successfully, false otherwise</returns>
    Task<bool> UpdateTickNote(long ccseq, string? note);

    /// <summary>
    /// Delete a single <see cref="MarketTick"/>.
    /// </summary>
    /// <param name="ccseq">The sequence number of the market tick to delete</param>
    /// <returns>True if the record was deleted, false if it was not found</returns>
    Task<bool> DeleteMarketTick(long ccseq);

    /// <summary>
    /// Delete multiple <see cref="MarketTick"/> records.
    /// </summary>
    /// <param name="ccseqs">Sequence numbers of the market ticks to delete</param>
    /// <returns>The number of deleted records</returns>
    Task<int> DeleteMarketTicks(IReadOnlyCollection<long> ccseqs);
}
