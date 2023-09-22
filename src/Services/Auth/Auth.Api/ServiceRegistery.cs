using BuildingBlocks.Messaging;
using BuildingBlocks.Security;
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
        services.AddServiceJwtAuthentication(config);

        //================================== CORS

        services.AddCors(x => x.AddPolicy("CORS_POLICY", opt =>
        {
            opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }));
    }
}