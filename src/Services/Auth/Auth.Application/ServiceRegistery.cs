using Auth.Application.Interfaces;
using Auth.Application.Mapping;
using Auth.Application.Models;
using Auth.Application.Services;
using BuildingBlocks.Security.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;

public static class ServiceRegistery
{
    public static void AddApplication(this IServiceCollection services)
    {
        //================================== Services
        services.AddAutoMapper(typeof(AuthMappers));

        services.AddValidatorsFromAssemblyContaining<EditProfileRequestValidator>(ServiceLifetime.Transient);

        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IAuthUserService, AuthUserService>();
        services.AddTransient<IJwtTokenFactory, JwtTokenFactory>();
        services.AddTransient<IAuthTokenStoreService, AuthTokenStoreService>();

        services.AddHttpContextAccessor();
    }
}