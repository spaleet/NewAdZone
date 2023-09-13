using BuildingBlocks.Core.Web;
using BuildingBlocks.Logging;
using Hellang.Middleware.ProblemDetails;
using Plan.Api;
using Plan.Api.Extensions;
using Plan.Application;
using Plan.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddGeneralConfiguration();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

await app.UseDbInitializer();

app.UseProblemDetails();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("CORS_POLICY");
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
