using BuildingBlocks.Core.Web.Extenions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.Web;

public static class GeneralConfiguration
{
    public static WebApplicationBuilder AddGeneralConfiguration(this WebApplicationBuilder builder)
    {
        builder.Services.AddRouting(options => options.LowercaseUrls = true);
        builder.Services.AddCustomProblemDetails();
        builder.Services.AddControllers();

        return builder;
    }
}
