using Auth.Api.Extensions;
using BuildingBlocks.Logging;

var builder = WebApplication.CreateBuilder(args);

builder.AddCustomSerilog();

builder.Services.ConfigureDb(builder.Configuration.GetConnectionString("SqlConnection")!);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.ConfigureServices();

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();
builder.Services.ConfigureSwagger();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
await app.UseDbInitializer();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();