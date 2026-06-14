using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BitcoinMarketLoader.Infrastructure.Databases.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketCurrencies",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketCurrencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketTicks",
                columns: table => new
                {
                    Ccseq = table.Column<long>(type: "bigint", nullable: false),
                    GeneratedTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Market = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Instrument = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    BaseCurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    QuoteCurrencyId = table.Column<long>(type: "bigint", nullable: true),
                    LastTrade_Ccseq = table.Column<long>(type: "bigint", nullable: true),
                    LastTrade_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastTrade_Quantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    LastTrade_QuoteQuantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    LastTrade_Price = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    LastTrade_Timestamp_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    LastTrade_Timestamp_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    LastTrade_ReceivedTimestamp_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    LastTrade_ReceivedTimestamp_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    LastTrade_Side = table.Column<int>(type: "int", nullable: true),
                    LastProcessedTrade_Ccseq = table.Column<long>(type: "bigint", nullable: true),
                    LastProcessedTrade_Id = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastProcessedTrade_Quantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    LastProcessedTrade_QuoteQuantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    LastProcessedTrade_Price = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    LastProcessedTrade_Timestamp_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    LastProcessedTrade_Timestamp_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    LastProcessedTrade_ReceivedTimestamp_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    LastProcessedTrade_ReceivedTimestamp_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    LastProcessedTrade_Side = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    PriceFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceUpdateTimestamp_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    PriceUpdateTimestamp_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    BestBid_Price = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    BestBid_Quantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    BestBid_QuoteQuantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    BestBid_LastUpdate_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    BestBid_LastUpdate_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    BestBid_PositionUpdate_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    BestBid_PositionUpdate_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    BestAsk_Price = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    BestAsk_Quantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    BestAsk_QuoteQuantity = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    BestAsk_LastUpdate_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    BestAsk_LastUpdate_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    BestAsk_PositionUpdate_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    BestAsk_PositionUpdate_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketTicks", x => x.Ccseq);
                    table.ForeignKey(
                        name: "FK_MarketTicks_MarketCurrencies_BaseCurrencyId",
                        column: x => x.BaseCurrencyId,
                        principalTable: "MarketCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MarketTicks_MarketCurrencies_QuoteCurrencyId",
                        column: x => x.QuoteCurrencyId,
                        principalTable: "MarketCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TradeStatistics",
                columns: table => new
                {
                    Range = table.Column<int>(type: "int", nullable: false),
                    SourceTickCcseq = table.Column<long>(type: "bigint", nullable: false),
                    VolumeTotal = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    VolumeBuy = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    VolumeSell = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    VolumeUnknown = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    QuoteVolumeTotal = table.Column<decimal>(type: "decimal(22,8)", precision: 22, scale: 8, nullable: true),
                    QuoteVolumeBuy = table.Column<decimal>(type: "decimal(22,8)", precision: 22, scale: 8, nullable: true),
                    QuoteVolumeSell = table.Column<decimal>(type: "decimal(22,8)", precision: 22, scale: 8, nullable: true),
                    QuoteVolumeUnknown = table.Column<decimal>(type: "decimal(22,8)", precision: 22, scale: 8, nullable: true),
                    OpenPrice = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    HighPrice = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    LowPrice = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    PriceChange = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    PriceChangePercent = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: true),
                    HighPriceTimestamp_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    HighPriceTimestamp_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    LowPriceTimestamp_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    LowPriceTimestamp_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    FirstTradeTimestamp_UnixSeconds = table.Column<long>(type: "bigint", nullable: true),
                    FirstTradeTimestamp_Nanoseconds = table.Column<long>(type: "bigint", nullable: true),
                    TotalTrades = table.Column<long>(type: "bigint", nullable: true),
                    TotalBuys = table.Column<long>(type: "bigint", nullable: true),
                    TotalSells = table.Column<long>(type: "bigint", nullable: true),
                    TotalUnknownTrades = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeStatistics", x => new { x.SourceTickCcseq, x.Range });
                    table.ForeignKey(
                        name: "FK_TradeStatistics_MarketTicks_SourceTickCcseq",
                        column: x => x.SourceTickCcseq,
                        principalTable: "MarketTicks",
                        principalColumn: "Ccseq",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketTicks_BaseCurrencyId",
                table: "MarketTicks",
                column: "BaseCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketTicks_QuoteCurrencyId",
                table: "MarketTicks",
                column: "QuoteCurrencyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeStatistics");

            migrationBuilder.DropTable(
                name: "MarketTicks");

            migrationBuilder.DropTable(
                name: "MarketCurrencies");
        }
    }
}
