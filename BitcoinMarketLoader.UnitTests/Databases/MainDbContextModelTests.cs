using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.Infrastructure.Databases;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BitcoinMarketLoader.UnitTests.Databases;

public class MainDbContextModelTests
{
    [Fact]
    public void ModelConfiguresMarketTickRelationshipsAndOwnedTimestamps()
    {
        var options = new DbContextOptionsBuilder<MainDbContext>()
            .UseSqlServer("Server=localhost;Database=ModelValidation;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        using var context = new MainDbContext(options);
        var marketTick = context.Model.FindEntityType(typeof(MarketTick));
        var tradeStatistics = context.Model.FindEntityType(typeof(TradeStatistics));
        var marketCurrency = context.Model.FindEntityType(typeof(MarketCurrency));

        Assert.NotNull(marketTick);
        Assert.NotNull(tradeStatistics);
        Assert.NotNull(marketCurrency);
        Assert.Equal(
            ValueGenerated.Never,
            marketCurrency.FindProperty(nameof(MarketCurrency.Id))?.ValueGenerated);

        AssertForeignKey(marketTick, nameof(MarketTick.BaseCurrency), "BaseCurrencyId");
        AssertForeignKey(marketTick, nameof(MarketTick.QuoteCurrency), "QuoteCurrencyId");
        AssertOwnedNavigation(marketTick, nameof(MarketTick.PriceUpdateTimestamp));

        var statisticsForeignKey = Assert.Single(
            tradeStatistics.GetForeignKeys(),
            foreignKey => foreignKey.PrincipalEntityType.ClrType == typeof(MarketTick));
        Assert.Equal(nameof(TradeStatistics.SourceTickCcseq), Assert.Single(statisticsForeignKey.Properties).Name);
        Assert.Equal(nameof(MarketTick.TradeStatistics), statisticsForeignKey.PrincipalToDependent?.Name);

        AssertOwnedNavigation(tradeStatistics, nameof(TradeStatistics.HighPriceTimestamp));
        AssertOwnedNavigation(tradeStatistics, nameof(TradeStatistics.LowPriceTimestamp));
        AssertOwnedNavigation(tradeStatistics, nameof(TradeStatistics.FirstTradeTimestamp));
    }

    private static void AssertForeignKey(
        IReadOnlyEntityType entityType,
        string navigationName,
        string foreignKeyName)
    {
        var navigation = entityType.FindNavigation(navigationName);
        Assert.NotNull(navigation);
        Assert.Equal(foreignKeyName, Assert.Single(navigation.ForeignKey.Properties).Name);
        Assert.Equal(DeleteBehavior.Restrict, navigation.ForeignKey.DeleteBehavior);
    }

    private static void AssertOwnedNavigation(
        IReadOnlyEntityType entityType,
        string navigationName)
    {
        var navigation = entityType.FindNavigation(navigationName);
        Assert.NotNull(navigation);
        Assert.True(navigation.TargetEntityType.IsOwned());
    }
}
