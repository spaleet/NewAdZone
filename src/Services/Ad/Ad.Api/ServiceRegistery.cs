namespace Ad.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }

}
