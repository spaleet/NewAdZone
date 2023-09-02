using Microsoft.OpenApi.Models;

namespace Ad.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Ad Api", Version = "v1" });
        });

        //================================== CORS

        services.AddCors(x => x.AddPolicy("CORS_POLICY", opt =>
        {
            opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }));
    }

}
