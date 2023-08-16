using MassTransit;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Messaging;

public static class ServiceRegistery
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, string connection)
    {
        services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, cfg) =>
            {
                cfg.Host(connection);
            });
        });

        return services;
    }
}
