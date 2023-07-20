using BuildingBlocks.Core.Validation;
using BuildingBlocks.Payment;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Plan.Application;
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

        services.AddZarinPal();
    }
}

public interface AssemblyMarker
{
}
