using System.Text.Json.Serialization;
using BuildingBlocks.Security;

namespace Ticket.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        //================================== Auth & Swagger
        services.AddServiceJwtAuthentication(configuration);
        services.AddSwaggerWithAuthentication("Ticket Api");

        //================================== CORS

        services.AddCors(x => x.AddPolicy("CORS_POLICY", opt =>
        {
            opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }));
    }
}