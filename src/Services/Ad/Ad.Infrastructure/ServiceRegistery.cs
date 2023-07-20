using Ad.Application.Interfaces;
using Ad.Infrastructure.Context;
using Ad.Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ad.Infrastructure;

public static class ServiceRegistery
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        //================================== Configure Db
        services.AddDbContext<AdDbContext>(
            opts => opts.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Ad.Infrastructure")));

        services.AddScoped<IAdDbContext>(provider => provider.GetService<AdDbContext>());

        services.AddScoped<AdDbInitializer>();
    }
}
