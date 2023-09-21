using BuildingBlocks.Core.Web;
using BuildingBlocks.Logging;
using Hellang.Middleware.ProblemDetails;
using Ticket.Api;
using Ticket.Api.Extensions;
using Ticket.Application;
using Ticket.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddGeneralConfiguration();

builder.Services.AddApplication();
builder.Services.AddInfrastructure();
builder.Services.AddApi();

var app = builder.Build();

app.UseDbInitializer();

app.UseProblemDetails();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
