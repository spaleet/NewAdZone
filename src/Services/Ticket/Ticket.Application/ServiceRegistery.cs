﻿using Microsoft.Extensions.DependencyInjection;

namespace Ticket.Application;

public static class ServiceRegistery
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(typeof(AssemblyMarker).Assembly);
        });

        services.AddAutoMapper(typeof(AssemblyMarker));
        services.AddValidatorsFromAssemblyContaining<AssemblyMarker>();
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        services.AddHttpContextAccessor();
    }
}

public interface AssemblyMarker
{
}