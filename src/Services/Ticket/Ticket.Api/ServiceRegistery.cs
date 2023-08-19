namespace Ticket.Api;

public static class ServiceRegistery
{
    public static void AddApi(this IServiceCollection services)
    {
        //================================== Swagger
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
    }
}
