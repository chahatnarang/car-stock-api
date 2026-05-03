using FastEndpoints;
using CarStock.Services;
using CarStock.Interfaces;
using FastEndpoints.Swagger;
using FastEndpoints.Security;
using DotNetEnv;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationJwtBearer(s => s.SigningKey = "The secret used to sign tokens")
.AddAuthorization()
.AddFastEndpoints()
.SwaggerDocument();

builder.Services.AddSingleton<IDatabaseConnection, DatabaseConnection>();
builder.Services.AddScoped<IDealerService, DealerService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.MapGet("/", () => "CAR STOCK API ONLINE!");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
DatabaseInitialiser.Initialise(connectionString);


app.UseAuthentication()
.UseAuthorization()
.UseFastEndpoints()
.UseSwaggerGen();

app.Run();
