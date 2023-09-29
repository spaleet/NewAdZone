using BuildingBlocks.Core.Web.Extenions;
using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;
using Serilog.Sinks.Async;

namespace BuildingBlocks.Logging;

public static class ServiceRegistery
{
    public static WebApplicationBuilder AddCustomSerilog(
        this WebApplicationBuilder builder,
        string sectionName = "Serilog")
    {
        var serilogOptions = builder.Configuration.BindOptions<SerilogOptions>(sectionName);

        // https://andrewlock.net/creating-a-rolling-file-logging-provider-for-asp-net-core-2-0/
        // https://github.com/serilog/serilog-extensions-hosting
        // https://andrewlock.net/adding-serilog-to-the-asp-net-core-generic-host/
        // Serilog replace `ILoggerFactory`,It replaces microsoft `LoggerFactory` class with `SerilogLoggerFactory`, so `ConsoleLoggerProvider` and other default microsoft logger providers don't instantiate at all with serilog
        builder.Host.UseSerilog(
            (context, serviceProvider, loggerConfiguration) =>
            {
                loggerConfiguration.Enrich
                    .WithProperty("Application", builder.Environment.ApplicationName)
                    .Enrich.WithEnvironmentName()
                    .Enrich.WithMachineName()
                    // https://rehansaeed.com/logging-with-serilog-exceptions/
                    .Enrich.WithExceptionDetails(
                        new DestructuringOptionsBuilder()
                            .WithDefaultDestructurers()
                            .WithDestructurers(new[] { new DbUpdateExceptionDestructurer() })
                    );

                // https://github.com/serilog/serilog-settings-configuration
                loggerConfiguration.ReadFrom.Configuration(context.Configuration, sectionName: sectionName);

                // write to console
                if (serilogOptions.UseConsole)
                {
                    loggerConfiguration.WriteTo.Async(writeTo =>
                            writeTo.Console(outputTemplate: serilogOptions.LogTemplate)
                    );
                }

                // write to file
                if (!string.IsNullOrEmpty(serilogOptions.LogPath))
                {
                    loggerConfiguration.WriteTo.Async(writeTo =>
                            writeTo.File(
                                serilogOptions.LogPath,
                                outputTemplate: serilogOptions.LogTemplate,
                                rollingInterval: RollingInterval.Day,
                                rollOnFileSizeLimit: true
                            )
                    );
                }
            }
        );

        return builder;
    }
}