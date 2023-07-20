using System.Net;
using System.Text;
using System.Text.Json;
using Auth.Application.Interfaces;
using Auth.Application.Mapping;
using Auth.Application.Services;
using Auth.Domain.Entities;
using Auth.Domain.Enums;
using Auth.Infrastructure.Context;
using Auth.Infrastructure.Seed;
using BuildingBlocks.Security;
using BuildingBlocks.Security.Interfaces;
using BuildingBlocks.Security.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Auth.Api.Extensions;

public static class ServiceRegistery
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "NewAdZone Auth APi", Version = "v1" });

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
    }

    public static void ConfigureDb(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<DatabaseContext>(
            opts => opts.UseSqlServer(connectionString,
                b => b.MigrationsAssembly("Auth.Infrastructure")));

        services.AddScoped<IAuthDbContext>(provider => provider.GetService<DatabaseContext>());

        services.AddScoped<AuthDbInitializer>();
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
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

    public static void ConfigureAuth(this IServiceCollection services, IConfiguration config)
    {
        var bearerTokenSettings =
            config.GetSection("BearerTokenSettings").Get(typeof(BearerTokenSettings)) as BearerTokenSettings;

        services.Configure<BearerTokenSettings>(config.GetSection("BearerTokenSettings"));

        services
            .AddAuthentication(options =>
            {
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = bearerTokenSettings?.Issuer,
                    ValidateIssuer = true,
                    ValidAudience = bearerTokenSettings?.Audiance,
                    ValidateAudience = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(bearerTokenSettings.Secret)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
                cfg.Events = new JwtBearerEvents
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

        services.AddAuthorization(options =>
        {
            options.AddPolicy(nameof(Roles.Admin), policy => policy.RequireRole(nameof(Roles.Admin)));
            options.AddPolicy(nameof(Roles.VerifiedUser), policy => policy.RequireRole(nameof(Roles.VerifiedUser)));
            options.AddPolicy(nameof(Roles.BasicUser), policy => policy.RequireRole(nameof(Roles.BasicUser)));
        });
    }

    public static void ConfigureServices(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AuthMappers));

        services.AddTransient<IAuthUserService, AuthUserService>();
        services.AddTransient<IJwtTokenFactory, JwtTokenFactory>();
        services.AddTransient<IAuthTokenStoreService, AuthTokenStoreService>();
        services.AddTransient<IAuthTokenValidatorService, AuthTokenValidatorService>();
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