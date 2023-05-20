using Auth.Api.Extensions;
using BuildingBlocks.Core.Web;
using BuildingBlocks.Logging;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddGeneralConfiguration();
builder.Services.ConfigureSwagger();

builder.Services.ConfigureDb(builder.Configuration.GetConnectionString("SqlConnection")!);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.ConfigureServices();

var app = builder.Build();

app.UseProblemDetails();

await app.UseDbInitializer();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();