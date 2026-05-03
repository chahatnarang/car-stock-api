using DotNetEnv;
using FastEndpoints;
using CarStock.Services;
using CarStock.Interfaces;
using FastEndpoints.Swagger;
using FastEndpoints.Security;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationJwtBearer(s => s.SigningKey = "The secret used to sign tokens")
.AddAuthorization()
.AddFastEndpoints()
.SwaggerDocument();

builder.Services.AddSingleton<IDatabaseConnection, DatabaseConnection>();
builder.Services.AddScoped<ICurrerntDealerService, CurrerntDealerService>();
builder.Services.AddScoped<IDealerService, DealerService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddHttpContextAccessor();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
DatabaseInitialiser.Initialise(connectionString);

var app = builder.Build();
app.MapGet("/", () => "CAR STOCK API ONLINE");

app.UseAuthentication()
.UseAuthorization()
.UseFastEndpoints()
.UseSwaggerGen();

app.UseHttpsRedirection();

app.Run();
