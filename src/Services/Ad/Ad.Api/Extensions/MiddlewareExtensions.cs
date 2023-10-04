using Ad.Infrastructure.Seed;

namespace Ad.Api.Extensions;

public static class MiddlewareExtensions
{
    public static void MigrateDatabase(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<AdDbInitializer>();

        initializer.Initialize();
    }
}