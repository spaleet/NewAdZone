namespace BuildingBlocks.Cache;

public interface ICacheService
{
    Task<T?> Get<T>(string key, CancellationToken cancellationToken = default) where T : class;
    Task<T> Get<T>(string key, Func<Task<T>> factory, CancellationToken cancellationToken = default) where T : class;

    Task Set<T>(string key, T value, CancellationToken cancellationToken = default) where T : class;

    Task Remove(string key, CancellationToken cancellationToken = default);
}
