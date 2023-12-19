using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Cache;

public static class ServiceRegistery
{
    public static IServiceCollection AddRedisCache(this IServiceCollection services, string connectionString)
    {
        //services.AddDistributedMemoryCache();

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = connectionString;
            options.InstanceName = "NewAdZone";
        });

        services.AddSingleton<ICacheService, CacheService>();

        return services;
    }
}