using System.ComponentModel.DataAnnotations;

namespace BitcoinMarketLoader.Domain.Market;

public class Instrument
{
    public int Id { get; set; }
    
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string? MappedInstrument { get; set; }

    [StringLength(10)]
    public required string BaseCurrency { get; set; }
    
    [StringLength(3)]
    public required string QuoteCurrency { get; set; }

    public required int BaseCurrencyId { get; set; }

    public required int QuoteCurrencyId { get; set; }
    
    [StringLength(100)]
    public string? TransformFunction { get; set; }
}