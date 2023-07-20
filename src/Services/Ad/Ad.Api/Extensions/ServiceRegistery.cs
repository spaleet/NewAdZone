using Ad.Application.Interfaces;
using Ad.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Ad.Api.Extensions;

public static class ServiceRegistery
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

    public static void ConfigureDb(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AdDbContext>(
            opts => opts.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Ad.Infrastructure")));

        services.AddScoped<IAdDbContext>(provider => provider.GetService<AdDbContext>());

        services.AddScoped<AdDbInitializer>();
    }
}
