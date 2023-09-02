using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Payment;
public static class ServiceRegistery
{
    public static void AddZarinPal(this IServiceCollection services)
    {
        services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
    }
}
