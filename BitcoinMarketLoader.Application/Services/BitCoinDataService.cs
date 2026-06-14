using System.Diagnostics;
using BitcoinMarketLoader.Application.Enums;
using BitcoinMarketLoader.Application.Models;
using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.Infrastructure.Databases;
using BitcoinMarketLoader.Infrastructure.Dtos.MarketTickDtos;
using BitcoinMarketLoader.Infrastructure.Extensions;
using BitcoinMarketLoader.Infrastructure.Http;
using Microsoft.Extensions.Logging;

namespace BitcoinMarketLoader.Application.Services;

public class BitCoinDataService(
    ILogger<BitCoinDataService> logger,
    ICoinDeskClient coinDeskClient,
    IMarketRepository marketRepository,
    IModelCurrencyConversionService currencyConversionService) : IBitCoinDataService
{
    private const string CoinbaseMarket = "coinbase";
    private const string BtcInstrument = "BTC-EUR";
    private const string EurCurrency = "EUR";
    private const string CzkCurrency = "CZK";

    public async Task<MarketTick?> FetchLatestBtcMarketTickAsync(
        CurrencyCodes currencyCodes,
        bool save = true)
    {
        MarketTickDto tickDto;
        try
        {
            tickDto = await coinDeskClient
                .GetLatestSpotTickAsync(CoinbaseMarket, [BtcInstrument])
                .ConfigureAwait(false);

            if (!string.IsNullOrEmpty(tickDto.Error?.Message))
            {
                logger.LogError("Last spot tick has error: {errorMsg}", tickDto.Error.Message);
                return null;
            }
        } catch (Exception ex)
        {
            logger.LogError(ex, "Error while fetching latest spot tick for BTC");
            return null;
        }

        MarketTick tickModel;
        try
        {
            tickModel = tickDto.ToDomainModel();
            tickModel.GeneratedTimestamp = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while converting spot tick to domain model for BTC");
            return null;
        }

        if (save)
        {
            try
            {
                await marketRepository
                    .AddMarketTick(tickModel)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error persisting market tick {tickId} for BTC", tickModel.Ccseq);
            }
        }

        await ConvertCurrencyAsync(tickModel, currencyCodes).ConfigureAwait(false);
        return tickModel;
    }

    public async Task<MarketTick?> GetBtcMarketTickAsync(
        long marketTickId,
        CurrencyCodes currencyCodes)
    {
        MarketTick? tick;
        try
        {
            tick = await marketRepository
                .GetMarketTick(marketTickId, true)
                .ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving spot tick {tickId} for BTC", marketTickId);
            return null;
        }

        if (tick is not null)
        {
            await ConvertCurrencyAsync(tick, currencyCodes).ConfigureAwait(false);
        }

        return tick;
    }
    
    public async Task<PagedResponse<MarketTick>> GetBtcMarketTicksAsync(
        CurrencyCodes currencyCodes,
        int page = 1,
        int pageSize = 10)
    {
        try
        {
            var totalCount = await marketRepository.GetMarketTicksCount().ConfigureAwait(false);
            var ticks = await marketRepository
                .GetMarketTicks((page - 1) * pageSize, pageSize, true)
                .ConfigureAwait(false);

            foreach (var tick in ticks)
            {
                await ConvertCurrencyAsync(tick, currencyCodes).ConfigureAwait(false);
            }

            return new PagedResponse<MarketTick>(ticks, page, pageSize, totalCount);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error while retrieving spot ticks for page {pageNumber} and page size {pageSize}", page, pageSize);
            throw;
        }
    }
    
    public async Task<bool> UpdateNoteForMarketTickAsync(long marketTickId, string? note)
    {
        try
        {
            var updated = await marketRepository
                .UpdateTickNote(marketTickId, note)
                .ConfigureAwait(false);
            if (!updated)
            {
                logger.LogWarning("Failed to update note of market tick {tickId}", marketTickId);
            }
            
            return updated;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating note of market tick {tickId}", marketTickId);
            return false;
        }
    }

    private Task<bool> ConvertCurrencyAsync(MarketTick tick, CurrencyCodes currencyCodes) => currencyCodes switch
    {
        CurrencyCodes.EUR => Task.FromResult(true), // No conversion needed
        CurrencyCodes.CZK => currencyConversionService.ConvertEurToCzk(tick),
        _ => throw new ArgumentOutOfRangeException(nameof(currencyCodes), $"Unsupported currency '{currencyCodes}'")
    };
}
