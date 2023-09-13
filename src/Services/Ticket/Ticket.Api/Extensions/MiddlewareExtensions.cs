using Ticket.Infrastructure.Seed;

namespace Ticket.Api.Extensions;

public static class MiddlewareExtensions
{
    public static void UseDbInitializer(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<TicketDbInitializer>();

        initializer.Initialize();
    }
}