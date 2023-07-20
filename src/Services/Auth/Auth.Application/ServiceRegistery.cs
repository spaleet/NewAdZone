using Auth.Application.Interfaces;
using Auth.Application.Mapping;
using Auth.Application.Services;
using BuildingBlocks.Security.Interfaces;
using BuildingBlocks.Security.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Auth.Application;
public static class ServiceRegistery
{
    public static void AddApplication(this IServiceCollection services)
    {
        //================================== Services
        services.AddAutoMapper(typeof(AuthMappers));

        services.AddTransient<IAuthUserService, AuthUserService>();
        services.AddTransient<IJwtTokenFactory, JwtTokenFactory>();
        services.AddTransient<IAuthTokenStoreService, AuthTokenStoreService>();
        services.AddTransient<IAuthTokenValidatorService, AuthTokenValidatorService>();
    }
}
