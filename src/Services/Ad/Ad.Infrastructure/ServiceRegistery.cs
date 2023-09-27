using Ad.Application.Clients;
using Ad.Application.Interfaces;
using Ad.Infrastructure.Clients;
using Ad.Infrastructure.Context;
using Ad.Infrastructure.Seed;
using Ad.Infrastructure.Settings;
using BuildingBlocks.Core.Web.Extenions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ad.Infrastructure;

public static class ServiceRegistery
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        //================================== Configure Db
        services.AddDbContext<AdDbContext>(
            opts => opts.UseSqlServer(configuration.GetConnectionString("SqlConnection"),
                b => b.MigrationsAssembly("Ad.Infrastructure")));

        services.AddScoped<IAdDbContext>(provider => provider.GetService<AdDbContext>());

        services.AddScoped<AdDbInitializer>();

        //================================== CLIENTS
        services.AddValidatedOptions<UserClientOptions>(nameof(UserClientOptions));

        services.AddHttpClient<IPlanClient, PlanClient>(client =>
        {
            client.BaseAddress = new Uri(configuration["UserClientOptions:UserPlanUrl"]!);
        });
    }
}
