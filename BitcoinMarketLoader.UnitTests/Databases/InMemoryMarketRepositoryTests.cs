using BitcoinMarketLoader.Domain.Enums;
using BitcoinMarketLoader.Domain.Market;
using BitcoinMarketLoader.Infrastructure.Databases.InMemory;

namespace BitcoinMarketLoader.UnitTests.Databases;

public class InMemoryMarketRepositoryTests
{
    [Fact]
    public async Task RepositorySupportsAddPagingLookupAndNoteUpdate()
    {
        var repository = new InMemoryMarketRepository();
        var firstTick = CreateMarketTick(20);
        var secondTick = CreateMarketTick(10);

        Assert.True(await repository.AddMarketTick(firstTick));
        Assert.True(await repository.AddMarketTick(secondTick));
        Assert.False(await repository.AddMarketTick(firstTick));
        Assert.Equal(2, await repository.GetMarketTicksCount());

        var page = await repository.GetMarketTicks(0, 1, true);
        var pagedTick = Assert.Single(page);
        Assert.Equal(10, pagedTick.Ccseq);
        Assert.Single(pagedTick.TradeStatistics!);

        Assert.True(await repository.UpdateTickNote(10, "updated"));
        Assert.False(await repository.UpdateTickNote(999, "missing"));

        var storedTick = await repository.GetMarketTick(10, true);
        Assert.NotNull(storedTick);
        Assert.Equal("updated", storedTick.Note);
        Assert.Single(storedTick.TradeStatistics!);
    }

    [Fact]
    public async Task ReadsReturnDetachedCopiesAndRespectIncludeDetails()
    {
        var repository = new InMemoryMarketRepository([CreateMarketTick(10)]);

        var withoutDetails = await repository.GetMarketTick(10);
        Assert.NotNull(withoutDetails);
        Assert.Null(withoutDetails.TradeStatistics);
        Assert.NotNull(withoutDetails.LastTrade);

        var withDetails = await repository.GetMarketTick(10, true);
        Assert.NotNull(withDetails);
        Assert.Single(withDetails.TradeStatistics!);

        withDetails.Note = "not persisted";
        withDetails.LastTrade!.Quantity = 999;
        withDetails.TradeStatistics!.Single().VolumeTotal = 999;

        var storedTick = await repository.GetMarketTick(10, true);
        Assert.NotNull(storedTick);
        Assert.Equal("note", storedTick.Note);
        Assert.Equal(1.5m, storedTick.LastTrade!.Quantity);
        Assert.Equal(100m, storedTick.TradeStatistics!.Single().VolumeTotal);
    }

    [Fact]
    public async Task DeleteMarketTickRemovesOnlyRequestedTick()
    {
        var repository = new InMemoryMarketRepository(
            [CreateMarketTick(10), CreateMarketTick(20)]);

        Assert.True(await repository.DeleteMarketTick(10));
        Assert.False(await repository.DeleteMarketTick(999));
        Assert.Null(await repository.GetMarketTick(10));
        Assert.NotNull(await repository.GetMarketTick(20));
    }

    [Fact]
    public async Task DeleteMarketTicksReturnsDeletedCountAndIgnoresDuplicatesAndMissingTicks()
    {
        var repository = new InMemoryMarketRepository(
            [CreateMarketTick(10), CreateMarketTick(20), CreateMarketTick(30)]);

        var deletedCount = await repository.DeleteMarketTicks([10, 10, 30, 999]);

        Assert.Equal(2, deletedCount);
        Assert.Equal(1, await repository.GetMarketTicksCount());
        Assert.NotNull(await repository.GetMarketTick(20));
    }

    private static MarketTick CreateMarketTick(long ccseq) =>
        new()
        {
            Ccseq = ccseq,
            Market = "coinbase",
            Instrument = "BTC-EUR",
            Note = "note",
            LastTrade = new TradeDetails
            {
                Ccseq = ccseq - 1,
                Quantity = 1.5m,
            },
            TradeStatistics =
            [
                new TradeStatistics
                {
                    SourceTickCcseq = ccseq,
                    Range = TimeWindowRanges.CurrentDay,
                    VolumeTotal = 100m,
                },
            ],
        };
}
