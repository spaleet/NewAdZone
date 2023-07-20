using Ad.Api.Extensions;
using BuildingBlocks.Core.Web;
using BuildingBlocks.Logging;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddGeneralConfiguration();

builder.Services.ConfigureSwagger();

var app = builder.Build();

app.UseProblemDetails();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
