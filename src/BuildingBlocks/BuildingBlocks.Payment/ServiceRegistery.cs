using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Payment;
public static class ServiceRegistery
{
    public static void AddZarinPal(this IServiceCollection services)
    {
        services.AddTransient<IZarinPalFactory, ZarinPalFactory>();
    }
}
