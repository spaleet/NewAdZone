using Ad.Api;
using Ad.Api.Extensions;
using Ad.Application;
using Ad.Infrastructure;
using BuildingBlocks.Logging;
using Hellang.Middleware.ProblemDetails;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddGeneralConfiguration();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration.GetConnectionString("SqlConnection"));
builder.Services.AddApi();

var app = builder.Build();

await app.UseDbInitializer();

app.UseProblemDetails();
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
