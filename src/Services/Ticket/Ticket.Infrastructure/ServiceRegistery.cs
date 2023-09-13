using BuildingBlocks.Persistence.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Ticket.Infrastructure.Context;
using Ticket.Infrastructure.Seed;

namespace Ticket.Infrastructure;

public static class ServiceRegistery
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddMongoDbContext<TicketDbContext>();

        services.AddScoped<TicketDbInitializer>();

    }
}
