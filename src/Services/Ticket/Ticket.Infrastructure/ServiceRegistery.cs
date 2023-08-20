using BuildingBlocks.Persistence.Mongo;
using Microsoft.Extensions.DependencyInjection;
using Ticket.Infrastructure.Context;

namespace Ticket.Infrastructure;

public static class ServiceRegistery
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddMongoDbContext<TicketDbContext>();
    }
}
