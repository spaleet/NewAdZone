using Auth.Infrastructure.Seed;

namespace Auth.Api.Extensions;

public static class MiddlewareExtensions
{
    public static async Task UseDbInitializer(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var initializer = scope.ServiceProvider.GetRequiredService<AuthDbInitializer>();

        await initializer.InitializeAsync();
    }
}