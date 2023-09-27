using BuildingBlocks.Security;
using MassTransit;
using Plan.Api.Consumers;

namespace Plan.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        // MassTransit
        string connection = configuration.GetConnectionString("EventBus");

        services.AddMassTransit(busConfig =>
        {
            busConfig.AddConsumer<UserCreatedConsumer>();

            busConfig.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(connection);

                cfg.ConfigureEndpoints(context);
            });
        });

        //================================== Auth & Swagger
        services.AddServiceJwtAuthentication(configuration);
        services.AddSwaggerWithAuthentication("Plan Api");

        //================================== CORS

        services.AddCors(x => x.AddPolicy("CORS_POLICY", opt =>
        {
            opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }));

    }
}
