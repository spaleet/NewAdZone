using System.Net;
using System.Text;
using System.Text.Json;
using Auth.Domain.Enums;
using BuildingBlocks.Messaging;
using BuildingBlocks.Security;
using BuildingBlocks.Security.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Auth.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services, IConfiguration config)
    {
        //MassTransit
        services.AddMessaging(config.GetConnectionString("EventBus") ?? "");

        //================================== Swagger
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth Api", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                Description = "Input your Bearer token in this format - Bearer {your token here} to access this API"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });

        //================================== Auth
        var bearerTokenSettings =
            config.GetSection("BearerTokenSettings").Get(typeof(BearerTokenSettings)) as BearerTokenSettings;

        services.Configure<BearerTokenSettings>(config.GetSection("BearerTokenSettings"));

        services.AddServiceJwtAuthentication(config);

        services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(Roles.Admin), policy => policy.RequireRole(nameof(Roles.Admin)));
            options.AddPolicy(nameof(Roles.VerifiedUser), policy => policy.RequireRole(nameof(Roles.VerifiedUser)));
            options.AddPolicy(nameof(Roles.BasicUser), policy => policy.RequireRole(nameof(Roles.BasicUser)));
        });

        //================================== CORS

        services.AddCors(x => x.AddPolicy("CORS_POLICY", opt =>
        {
            opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }));
    }
}