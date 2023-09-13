using Plan.Infrastructure.Seed;

namespace Plan.Api.Extensions;

public static class MiddlewareExtensions
{
    public static async Task UseDbInitializer(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<PlanDbInitializer>();

        await initializer.InitializeAsync();
    }
}