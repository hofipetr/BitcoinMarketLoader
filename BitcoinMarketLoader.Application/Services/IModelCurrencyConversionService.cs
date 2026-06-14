using BitcoinMarketLoader.Domain.Market;

namespace BitcoinMarketLoader.Application.Services;

/// <summary>
/// Service for converting currencies in models using relevant exchange rates. 
/// </summary>
public interface IModelCurrencyConversionService
{
    /// <summary>
    /// Converts prices in <see cref="MarketTick"/> from EUR to CZK according to relevant exchange rates
    /// </summary>
    /// <param name="model"><see cref="MarketTick"/> instance to convert</param>
    /// <returns>True if the conversion was successful, false otherwise. If the result is false, source model may be converted partially.</returns>
    Task<bool> ConvertEurToCzk(MarketTick model);
}