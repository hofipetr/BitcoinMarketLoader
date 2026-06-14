using BitcoinMarketLoader.Application.Configurations;
using BitcoinMarketLoader.Application.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BitcoinMarketLoader.Application.Services;

public sealed class MarketTickPollingService(
    IServiceScopeFactory scopeFactory,
    IOptions<MarketTickPollingOptions> options,
    ILogger<MarketTickPollingService> logger) : IMarketTickPollingService
{
    public int PollingIntervalSeconds => Math.Max(0, options.Value.IntervalSeconds);

    public async Task StartPollingAsync(CancellationToken cancellationToken)
    {
        if (PollingIntervalSeconds <= 0)
        {
            logger.LogInformation("BTC market tick polling is disabled");
            return;
        }

        logger.LogInformation(
            "BTC market tick polling started with interval {intervalSeconds} seconds",
            PollingIntervalSeconds);

        await FetchAndPersistLatestTickAsync(cancellationToken).ConfigureAwait(false);

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(PollingIntervalSeconds));
        try
        {
            while (await timer.WaitForNextTickAsync(cancellationToken).ConfigureAwait(false))
            {
                await FetchAndPersistLatestTickAsync(cancellationToken).ConfigureAwait(false);
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            logger.LogInformation("BTC market tick polling stopped");
        }
    }

    private async Task FetchAndPersistLatestTickAsync(CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested)
        {
            return;
        }

        try
        {
            await using var scope = scopeFactory.CreateAsyncScope();
            var bitCoinDataService = scope.ServiceProvider
                .GetRequiredService<IBitCoinDataService>();
            var tick = await bitCoinDataService
                .FetchLatestBtcMarketTickAsync(CurrencyCodes.EUR, save: true)
                .ConfigureAwait(false);

            if (tick is null)
            {
                logger.LogWarning("Scheduled BTC market tick retrieval returned no data");
                return;
            }

            logger.LogDebug(
                "Scheduled BTC market tick {tickId} was fetched",
                tick.Ccseq);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Error during scheduled BTC market tick retrieval");
        }
    }
}
