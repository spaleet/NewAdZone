using BuildingBlocks.Core.Web.Extenions;
using BuildingBlocks.Persistence.Mongo.Base;
using BuildingBlocks.Persistence.Mongo.DbContext;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Persistence.Mongo;
public static class ServiceRegistery
{
    public static WebApplicationBuilder AddMongoDbContext<TContext>(this WebApplicationBuilder builder)
        where TContext : MongoDbContext, IMongoDbContext
    {
        var serilogOptions = builder.Configuration.BindOptions<MongoOptions>(nameof(MongoOptions));

        builder.Services.AddScoped(typeof(TContext));
        builder.Services.AddScoped<IMongoDbContext>(sp => sp.GetRequiredService<TContext>());

        return builder;
    }
}

