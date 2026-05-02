using FastEndpoints;
using FastEndpoints.Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthenticationJwtBearer(s => s.SigningKey = "The secret used to sign tokens")
.AddAuthorization()
.AddFastEndpoints();

var app = builder.Build();
app.MapGet("/", () => "CAR STOCK API ONLINE!");

app.UseAuthentication()
.UseAuthorization()
.UseFastEndpoints();

app.Run();
