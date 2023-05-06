using Auth.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureDb(builder.Configuration.GetConnectionString("SqlConnection")!);
builder.Services.ConfigureIdentity();
builder.Services.ConfigureAuth(builder.Configuration);
builder.Services.ConfigureServices();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
