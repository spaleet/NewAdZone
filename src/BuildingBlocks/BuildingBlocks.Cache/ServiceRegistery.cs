using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Cache;

public static class ServiceRegistery
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services)
    {
        services.AddDistributedMemoryCache();
        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}