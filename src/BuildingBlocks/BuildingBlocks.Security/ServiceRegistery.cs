using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using BuildingBlocks.Security.Interfaces;
using Auth.Application.Services;

namespace BuildingBlocks.Security;

public static class ServiceRegistery
{
    public static void AddServiceJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        // add services
        services.AddTransient<IAuthTokenValidatorService, AuthTokenValidatorService>();

        // add options
        services.AddOptions<BearerTokenSettings>()
            .BindConfiguration("BearerTokenSettings")
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var bearerTokenSettings =
            config.GetSection("BearerTokenSettings").Get(typeof(BearerTokenSettings)) as BearerTokenSettings;


        // add auth
        services
            .AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = bearerTokenSettings?.Issuer,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearerTokenSettings.Secret)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(ProduceUnAuthorizedResponse());
                    },
                    OnTokenValidated = context =>
                    {
                        var tokenValidatorService =
                            context.HttpContext.RequestServices.GetRequiredService<IAuthTokenValidatorService>();
                        return tokenValidatorService.ValidateAsync(context);
                    },
                    OnMessageReceived = context => { return Task.CompletedTask; },
                    OnChallenge = context =>
                    {
                        context.HandleResponse();
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(ProduceUnAuthorizedResponse());
                    },
                    OnForbidden = context =>
                    {
                        context.Response.StatusCode = 403;
                        context.Response.ContentType = "application/json";
                        return context.Response.WriteAsync(ProduceAccessDeniedResponse());
                    }
                };
            });
    }

    private static string ProduceUnAuthorizedResponse()
    {
        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Un Authorized",
            Status = (int)HttpStatusCode.Unauthorized,
            Detail = "Un Authorized!"
        };
        return JsonSerializer.Serialize(problemDetails);
    }

    private static string ProduceAccessDeniedResponse()
    {
        var problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Title = "Access Denied",
            Status = (int)HttpStatusCode.Forbidden,
            Detail = "Access Denied!"
        };
        return JsonSerializer.Serialize(problemDetails);
    }
}