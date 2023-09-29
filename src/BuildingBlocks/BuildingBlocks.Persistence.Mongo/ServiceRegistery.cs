using BuildingBlocks.Core.Web.Extenions;
using BuildingBlocks.Persistence.Mongo.Base;
using BuildingBlocks.Persistence.Mongo.DbContext;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Persistence.Mongo;

public static class ServiceRegistery
{
    public static IServiceCollection AddMongoDbContext<TContext>(this IServiceCollection services)
        where TContext : MongoDbContext, IMongoDbContext
    {
        services.AddValidatedOptions<MongoOptions>(nameof(MongoOptions));

        services.AddScoped(typeof(TContext));
        services.AddScoped<IMongoDbContext>(sp => sp.GetRequiredService<TContext>());

        return services;
    }
}