using Ad.Infrastructure.Seed;

namespace Ad.Api.Extensions;

public static class MiddlewareExtensions
{
    public static async Task UseDbInitializer(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<AdDbInitializer>();

        await initializer.InitializeAsync();
    }
}