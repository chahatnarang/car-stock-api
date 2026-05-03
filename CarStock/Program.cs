using DotNetEnv;
using FastEndpoints;
using CarStock.Services;
using CarStock.Interfaces;
using FastEndpoints.Swagger;
using FastEndpoints.Security;

Env.Load();

var builder = WebApplication.CreateBuilder(args);

builder.Services
.AddAuthenticationJwtBearer(s => s.SigningKey = builder.Configuration["Jwt:SigningKey"]!)
.AddAuthorization()
.AddFastEndpoints()
.SwaggerDocument(o => o.DocumentSettings = s =>
    {
        s.Title = "Car Stock API";
        s.Version = "v1";
    });

builder.Services.AddSingleton<IDatabaseConnectionFactory, DatabaseConnectionFactory>();
builder.Services.AddScoped<ICurrentDealerService, CurrentDealerService>();
builder.Services.AddScoped<IDealerService, DealerService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.MapGet("/", () => "CAR STOCK API ONLINE");

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
DatabaseInitialiser.Initialise(connectionString);

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseSwaggerGen();

app.UseFastEndpoints(c =>
{
    c.Errors.ResponseBuilder = (failures, ctx, statusCode) => new
    {
        StatusCode = statusCode,
        Message = "One or more validation errors occurred.",
        Errors = failures
            .GroupBy(f => f.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(f => f.ErrorMessage).ToArray())
    };
});

app.Run();
