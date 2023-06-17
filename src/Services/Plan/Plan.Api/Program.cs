using BuildingBlocks.Core.Web;
using BuildingBlocks.Logging;
using BuildingBlocks.Persistence.Mongo;
using Hellang.Middleware.ProblemDetails;
using Plan.Infrastructure.Context;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();
builder.AddGeneralConfiguration();

builder.Services.AddMongoDbContext<PlanDbContext>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseProblemDetails();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
