
using System.Text.Json;
using System.Text.Json.Serialization;
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
        string? result = await _distributedCache.GetStringAsync(key, cancellationToken);

        if (result is null) return null;
        
        return JsonSerializer.Deserialize<T>(result);
    }

    public Task<T> Get<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class
    {
        throw new NotImplementedException();
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
