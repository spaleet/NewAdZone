using Auth.Application.Interfaces;
using Auth.Domain.Entities;
using Auth.Infrastructure.Context;
using Auth.Infrastructure.Seed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Infrastructure;

public static class ServiceRegistery
{
    public static void AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        //================================== Configure Db
        services.AddDbContext<DatabaseContext>(
            opts => opts.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Auth.Infrastructure")));

        services.AddScoped<IAuthDbContext>(provider => provider.GetService<DatabaseContext>());

        services.AddScoped<AuthDbInitializer>();

        //================================== Configure Identity
        services.AddIdentity<User, UserRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 4;
            options.Password.RequiredUniqueChars = 0;
            options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<DatabaseContext>()
            .AddDefaultTokenProviders();
    }
}