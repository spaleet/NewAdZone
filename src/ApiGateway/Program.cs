using BuildingBlocks.Logging;
using BuildingBlocks.Security;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

// auth
builder.Services.AddServiceJwtAuthentication(builder.Configuration);

var app = builder.Build();

await app.UseOcelot();

app.UseAuthentication();
app.UseAuthorization();

app.Run();