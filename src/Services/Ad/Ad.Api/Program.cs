using Ad.Api;
using Ad.Api.Extensions;
using Ad.Application;
using Ad.Infrastructure;
using BuildingBlocks.Logging;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddGeneralConfiguration();

builder.Services.AddApplication(builder.Configuration);
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

app.MigrateDatabase();

app.UseProblemDetails();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Ad Api");
    c.DisplayRequestDuration();
});

app.UseCors("CORS_POLICY");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();