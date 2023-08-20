using System.Text.RegularExpressions;
using BuildingBlocks.Core.Web.Extenions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.Web;

public static class GeneralConfiguration
{
    public static WebApplicationBuilder AddGeneralConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddCustomProblemDetails();
        builder.Services.AddControllers();
        builder.Services.AddControllers(options =>
        {
            options.Conventions.Add(new RouteTokenTransformerConvention(new SlugifyParameterTransformer()));
        });

        return builder;
    }
}
