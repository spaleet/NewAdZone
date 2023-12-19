namespace BuildingBlocks.Cache;

public interface ICacheService
{
    /// <summary>
    /// Get value from cache, if value doesn't exists use `factory` function to retrieve value and set to cache
    /// </summary>
    /// <param name="key">cache key</param>
    Task<T?> Get<T>(string key) where T : class;


    /// <summary>
    /// Get value from cache, if value doesn't exists use `factory` function to retrieve value and set to cache
    /// </summary>
    /// <param name="key">cache key</param>
    /// <param name="factory">Function to get value</param>
    /// <returns></returns>
    Task<T> Get<T>(string key, Func<Task<T>> factory, TimeSpan slidingTime) where T : class;


    /// <summary>
    /// Set value to cache
    /// </summary>
    /// <param name="key">cache key</param>
    Task Set<T>(string key, T value, TimeSpan slidingTime) where T : class;

    /// <summary>
    /// Remove value from cache
    /// </summary>
    /// <param name="key">cache key</param>
    Task Remove(string key);
}
