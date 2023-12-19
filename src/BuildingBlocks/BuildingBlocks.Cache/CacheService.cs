
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace BuildingBlocks.Cache;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _distributedCache;

    public CacheService(IDistributedCache distributedCache)
    {
        _distributedCache = distributedCache;
    }

    public async Task<T?> Get<T>(string key, CancellationToken cancellationToken = default) where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (cachedValue is null) return null;
        
        return JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task<T> Get<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        T? cachedValue = await Get<T>(key, cancellationToken);

        if (cachedValue is not null) return cachedValue;

        cachedValue = await factory();

        await Set(key, cachedValue, cancellationToken);

        return cachedValue;
    }

    public async Task Set<T>(string key, T value, CancellationToken cancellationToken = default) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value);

        await _distributedCache.SetStringAsync(key, cacheValue, cancellationToken);
    }

    public async Task Remove(string key, CancellationToken cancellationToken = default)
    {
        await _distributedCache.RemoveAsync(key);
    }
}
