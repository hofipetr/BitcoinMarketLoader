namespace BitcoinMarketLoader.Application.Models;

public record class PagedResponse<T>(
    ICollection<T> Data,
    int Page,
    int PageSize,
    int TotalCount
);