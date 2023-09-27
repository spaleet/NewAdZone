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
using BuildingBlocks.Security.Services;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;

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

        // authorization
        services.AddAuthorization(options =>
        {
            options.AddPolicy(AuthConsts.Admin, policy => policy.RequireRole("Admin"));
            options.AddPolicy(AuthConsts.User, policy => policy.RequireRole("User"));
        });
    }

    public static void AddSwaggerWithAuthentication(this IServiceCollection services, string title)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = title, Version = "v1" });

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

            c.OperationFilter<IgnorePropertyFilter>();
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

    public class IgnorePropertyFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {

            var excludedProperties = context.ApiDescription.ParameterDescriptions.Where(p =>
                p.Source.Equals(BindingSource.Form));

            if (excludedProperties.Any())
            {

                foreach (var excludedPropertie in excludedProperties)
                {
                    foreach (var customAttribute in excludedPropertie.CustomAttributes())
                    {
                        if (customAttribute.GetType() == typeof(JsonIgnoreAttribute))
                        {
                            for (int i = 0; i < operation.RequestBody.Content.Values.Count; i++)
                            {
                                for (int j = 0; j < operation.RequestBody.Content.Values.ElementAt(i).Encoding.Count; j++)
                                {
                                    if (operation.RequestBody.Content.Values.ElementAt(i).Encoding.ElementAt(j).Key ==
                                        excludedPropertie.Name)
                                    {
                                        operation.RequestBody.Content.Values.ElementAt(i).Encoding
                                            .Remove(operation.RequestBody.Content.Values.ElementAt(i).Encoding
                                                .ElementAt(j));
                                        operation.RequestBody.Content.Values.ElementAt(i).Schema.Properties.Remove(excludedPropertie.Name);


                                    }
                                }
                            }

                        }
                    }
                }

            }
        }
    }
}