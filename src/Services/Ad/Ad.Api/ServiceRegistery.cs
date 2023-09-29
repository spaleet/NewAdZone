using BuildingBlocks.Security;

namespace Ad.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services, IConfiguration config)
    {
        //================================== Auth & Swagger
        services.AddServiceJwtAuthentication(config);
        services.AddSwaggerWithAuthentication("Ad Api");

        //================================== CORS

        services.AddCors(x => x.AddPolicy("CORS_POLICY", opt =>
        {
            opt.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
        }));
    }
}