using Microsoft.EntityFrameworkCore;
using BitcoinMarketLoader.Domain.Market;

namespace BitcoinMarketLoader.Infrastructure.Databases;

public class MainDbContext(DbContextOptions<MainDbContext> options) : DbContext(options)
{
    public DbSet<MarketTick> MarketTicks { get; set; }
    public DbSet<TradeStatistics> TradeStatistics { get; set; }
    public DbSet<MarketCurrency> MarketCurrencies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MarketTick>(b =>
        {
            b.HasKey(m => m.Ccseq);
            b.Property(m => m.Ccseq)
                .ValueGeneratedNever();

            b.HasOne(m => m.BaseCurrency)
                .WithMany()
                .HasForeignKey("BaseCurrencyId")
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(m => m.QuoteCurrency)
                .WithMany()
                .HasForeignKey("QuoteCurrencyId")
                .OnDelete(DeleteBehavior.Restrict);

            b.OwnsOne(m => m.PriceUpdateTimestamp, mt =>
            {
                mt.Property(p => p.UnixSeconds).HasColumnName("PriceUpdateTimestamp_UnixSeconds");
                mt.Property(p => p.Nanoseconds).HasColumnName("PriceUpdateTimestamp_Nanoseconds");
            });

            // Owned types: LastTrade
            b.OwnsOne(m => m.LastTrade, td =>
            {
                td.WithOwner().HasForeignKey("MarketTickCcseq");
                td.HasKey("MarketTickCcseq");

                td.Property(t => t.TradeId).HasColumnName("LastTrade_Id");
                td.Property(t => t.Quantity).HasColumnName("LastTrade_Quantity");
                td.Property(t => t.QuoteQuantity).HasColumnName("LastTrade_QuoteQuantity");
                td.Property(t => t.Price).HasColumnName("LastTrade_Price");
                td.Property(t => t.Ccseq).HasColumnName("LastTrade_Ccseq");
                td.Property(t => t.Side).HasColumnName("LastTrade_Side");

                td.OwnsOne(t => t.Timestamp, mt =>
                {
                    mt.Property(p => p.UnixSeconds).HasColumnName("LastTrade_Timestamp_UnixSeconds");
                    mt.Property(p => p.Nanoseconds).HasColumnName("LastTrade_Timestamp_Nanoseconds");
                });

                td.OwnsOne(t => t.ReceivedTimestamp, mt =>
                {
                    mt.Property(p => p.UnixSeconds).HasColumnName("LastTrade_ReceivedTimestamp_UnixSeconds");
                    mt.Property(p => p.Nanoseconds).HasColumnName("LastTrade_ReceivedTimestamp_Nanoseconds");
                });
            });

            // Owned types: LastProcessedTrade
            b.OwnsOne(m => m.LastProcessedTrade, td =>
            {
                td.WithOwner().HasForeignKey("MarketTickCcseq");
                td.HasKey("MarketTickCcseq");

                td.Property(t => t.TradeId).HasColumnName("LastProcessedTrade_Id");
                td.Property(t => t.Quantity).HasColumnName("LastProcessedTrade_Quantity");
                td.Property(t => t.QuoteQuantity).HasColumnName("LastProcessedTrade_QuoteQuantity");
                td.Property(t => t.Price).HasColumnName("LastProcessedTrade_Price");
                td.Property(t => t.Ccseq).HasColumnName("LastProcessedTrade_Ccseq");
                td.Property(t => t.Side).HasColumnName("LastProcessedTrade_Side");

                td.OwnsOne(t => t.Timestamp, mt =>
                {
                    mt.Property(p => p.UnixSeconds).HasColumnName("LastProcessedTrade_Timestamp_UnixSeconds");
                    mt.Property(p => p.Nanoseconds).HasColumnName("LastProcessedTrade_Timestamp_Nanoseconds");
                });

                td.OwnsOne(t => t.ReceivedTimestamp, mt =>
                {
                    mt.Property(p => p.UnixSeconds).HasColumnName("LastProcessedTrade_ReceivedTimestamp_UnixSeconds");
                    mt.Property(p => p.Nanoseconds).HasColumnName("LastProcessedTrade_ReceivedTimestamp_Nanoseconds");
                });
            });

            // Owned types: BestBid
            b.OwnsOne(m => m.BestBid, ob =>
            {
                ob.Property(o => o.Price).HasColumnName("BestBid_Price");
                ob.Property(o => o.Quantity).HasColumnName("BestBid_Quantity");
                ob.Property(o => o.QuoteQuantity).HasColumnName("BestBid_QuoteQuantity");

                ob.OwnsOne(o => o.LastUpdateTimestamp, mt =>
                {
                    mt.Property(p => p.UnixSeconds).HasColumnName("BestBid_LastUpdate_UnixSeconds");
                    mt.Property(p => p.Nanoseconds).HasColumnName("BestBid_LastUpdate_Nanoseconds");
                });

                ob.OwnsOne(o => o.PositionUpdateTimestamp, mt =>
                {
                    mt.Property(p => p.UnixSeconds).HasColumnName("BestBid_PositionUpdate_UnixSeconds");
                    mt.Property(p => p.Nanoseconds).HasColumnName("BestBid_PositionUpdate_Nanoseconds");
                });
            });
            b.Navigation(m => m.BestBid).IsRequired();

            // Owned types: BestAsk
            b.OwnsOne(m => m.BestAsk, ob =>
            {
                ob.Property(o => o.Price).HasColumnName("BestAsk_Price");
                ob.Property(o => o.Quantity).HasColumnName("BestAsk_Quantity");
                ob.Property(o => o.QuoteQuantity).HasColumnName("BestAsk_QuoteQuantity");

                ob.OwnsOne(o => o.LastUpdateTimestamp, mt =>
                {
                    mt.Property(p => p.UnixSeconds).HasColumnName("BestAsk_LastUpdate_UnixSeconds");
                    mt.Property(p => p.Nanoseconds).HasColumnName("BestAsk_LastUpdate_Nanoseconds");
                });

                ob.OwnsOne(o => o.PositionUpdateTimestamp, mt =>
                {
                    mt.Property(p => p.UnixSeconds).HasColumnName("BestAsk_PositionUpdate_UnixSeconds");
                    mt.Property(p => p.Nanoseconds).HasColumnName("BestAsk_PositionUpdate_Nanoseconds");
                });
            });
            b.Navigation(m => m.BestAsk).IsRequired();

            b.Property(m => m.Note).HasMaxLength(1024);
        });

        modelBuilder.Entity<TradeStatistics>(b =>
        {
            // Composite key (SourceTickCcseq + Range)
            b.HasKey(t => new { t.SourceTickCcseq, t.Range });

            b.HasOne(t => t.SourceTick)
                .WithMany(m => m.TradeStatistics)
                .HasForeignKey(t => t.SourceTickCcseq)
                .OnDelete(DeleteBehavior.Cascade);
            
            b.OwnsOne(t => t.HighPriceTimestamp, mt =>
            {
                mt.Property(p => p.UnixSeconds).HasColumnName("HighPriceTimestamp_UnixSeconds");
                mt.Property(p => p.Nanoseconds).HasColumnName("HighPriceTimestamp_Nanoseconds");
            });

            b.OwnsOne(t => t.LowPriceTimestamp, mt =>
            {
                mt.Property(p => p.UnixSeconds).HasColumnName("LowPriceTimestamp_UnixSeconds");
                mt.Property(p => p.Nanoseconds).HasColumnName("LowPriceTimestamp_Nanoseconds");
            });

            b.OwnsOne(t => t.FirstTradeTimestamp, mt =>
            {
                mt.Property(p => p.UnixSeconds).HasColumnName("FirstTradeTimestamp_UnixSeconds");
                mt.Property(p => p.Nanoseconds).HasColumnName("FirstTradeTimestamp_Nanoseconds");
            });
        });

        modelBuilder.Entity<MarketCurrency>(b =>
        {
            b.HasKey(c => c.Id);
            b.Property(c => c.Id).ValueGeneratedNever();
            b.Property(c => c.Name).HasMaxLength(3);
        });

        base.OnModelCreating(modelBuilder);
    }
}
