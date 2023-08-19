using System.Text.Json.Serialization;

namespace Ticket.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services)
    {
        services.AddControllers()
            .AddJsonOptions(options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));
        
        //================================== Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
