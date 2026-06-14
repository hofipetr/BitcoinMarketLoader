using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace BitcoinMarketLoader.Infrastructure.Http;

public interface IBaseHttpClient
{
    Task<TResult?> GetAsync<TResult>(string path, IDictionary<string, string>? queryParams = null, CancellationToken cancellationToken = default);

    Task<TResult?> PostAsync<TBody, TResult>(string path, TBody requestBody, CancellationToken cancellationToken = default);
}
