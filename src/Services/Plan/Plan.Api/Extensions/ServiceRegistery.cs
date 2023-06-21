using BuildingBlocks.Persistence.Mongo;
using FluentValidation;
using Plan.Application;
using Plan.Infrastructure.Context;

namespace Plan.Api.Extensions;

public static class ServiceRegistery
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    // TODO : rename to AddInfrastracure
    public static void ConfigureDb(this IServiceCollection services)
    {
        services.AddMongoDbContext<PlanDbContext>();
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(AssemblyMarker).Assembly);
        });

        services.AddAutoMapper(typeof(AssemblyMarker));
        services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();
    }


}
