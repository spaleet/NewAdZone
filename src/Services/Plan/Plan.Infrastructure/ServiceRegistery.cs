using BuildingBlocks.Persistence.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Plan.Infrastructure.Context;

namespace Plan.Infrastructure;

public static class ServiceRegistery
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddMongoDbContext<PlanDbContext>();
    }
}
