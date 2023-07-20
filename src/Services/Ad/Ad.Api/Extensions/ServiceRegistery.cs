namespace Ad.Api.Extensions;

public static class ServiceRegistery
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
