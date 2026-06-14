using System.ComponentModel.DataAnnotations;

namespace BitcoinMarketLoader.Domain.Market;

public class MarketCurrency
{
    [Key]
    public long Id { get; set; }
    
    [StringLength(3)]
    public string? Name { get; set; }
}