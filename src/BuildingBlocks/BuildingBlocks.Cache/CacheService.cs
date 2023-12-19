
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

    public async Task<T?> Get<T>(string key) where T : class
    {
        string? cachedValue = await _distributedCache.GetStringAsync(key);

        if (cachedValue is null) return null;
        
        return JsonSerializer.Deserialize<T>(cachedValue);
    }

    public async Task<T> Get<T>(string key, Func<Task<T>> factory, TimeSpan slidingTime) where T : class
    {
        T? cachedValue = await Get<T>(key);

        if (cachedValue is not null) return cachedValue;

        cachedValue = await factory();

        await Set(key, cachedValue, slidingTime);

        return cachedValue;
    }

    public async Task Set<T>(string key, T value, TimeSpan slidingTime) where T : class
    {
        string cacheValue = JsonSerializer.Serialize(value);

        var options = new DistributedCacheEntryOptions
        {
            SlidingExpiration = slidingTime
        };

        await _distributedCache.SetStringAsync(key, cacheValue, options);
    }

    public async Task Remove(string key)
    {
        await _distributedCache.RemoveAsync(key);
    }
}
