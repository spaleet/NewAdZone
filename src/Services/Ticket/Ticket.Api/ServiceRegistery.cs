namespace Ticket.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        //================================== Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
