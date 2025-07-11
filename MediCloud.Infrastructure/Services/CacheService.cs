using System.Collections.Concurrent;
using System.Text.Json;
using MediCloud.Application.Common.Interfaces.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace MediCloud.Infrastructure.Services;

public class CacheService(
    IDistributedCache distributedCache
) : ICacheService {

    private readonly static ConcurrentDictionary<string, bool> CacheKeys = new();

    public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken = default) where T : class {
        string? cachedValue = await distributedCache.GetStringAsync(key, cancellationToken);
        return cachedValue == null ? null : JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task<T> GetAsync<T>(
        string            key,
        Func<Task<T>>     factory,
        TimeSpan          relativeExpiration = default,
        CancellationToken cancellationToken  = default
    )
        where T : class {
        if (await GetAsync<T>(key, cancellationToken) is { } value)
            return value;

        T cacheValue = await factory();
        await SetAsync(key, cacheValue, relativeExpiration, cancellationToken);
        return cacheValue;
    }

    public async Task SetAsync<T>(
        string            key,
        T                 value,
        DateTimeOffset?   absExpiration     = null,
        CancellationToken cancellationToken = default
    ) where T : class {
        string cacheValue = JsonSerializer.Serialize(value);

        DistributedCacheEntryOptions options = new() {
            AbsoluteExpiration = absExpiration
        };
        await distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);
    }

    public Task SetAsync<T>(
        string            key,
        T                 value,
        TimeSpan          relativeExpiration,
        CancellationToken cancellationToken = default
    ) where T : class {
        string cacheValue = JsonSerializer.Serialize(value);

        DistributedCacheEntryOptions options = new() {
            AbsoluteExpirationRelativeToNow = relativeExpiration
        };
        return distributedCache.SetStringAsync(key, cacheValue, options, cancellationToken);
    }

    public async Task RemoveAsync(string key, CancellationToken cancellationToken = default) {
        await distributedCache.RemoveAsync(key, cancellationToken);

        CacheKeys.TryRemove(key, out _);
    }

    public Task RemoveByPrefixAsync(string prefix, CancellationToken cancellationToken = default) {
        var tasks = CacheKeys.Keys
                             .Where(k => k.StartsWith(prefix))
                             .Select(k => RemoveAsync(k, cancellationToken));
        return Task.WhenAll(tasks);
    }

}
