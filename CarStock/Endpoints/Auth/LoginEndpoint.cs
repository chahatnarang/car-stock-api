using FastEndpoints;
using CarStock.Response;
using FastEndpoints.Security;

namespace CarStock.Endpoints.Auth;

public class LoginRequest
{
    public string Email {get; init;} = "";
    public string Password {get; init;} = "";
}

public class LoginEndpoint : Endpoint<LoginRequest, LoginResponse>
{
    private readonly IDealerService _dealerService;
    private readonly IConfiguration _configuration;

    public LoginEndpoint(IDealerService dealerService, IConfiguration configuration)
    {
        _dealerService = dealerService;
        _configuration = configuration;
    }

    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
        Description(d => d.WithTags("Auth"));
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var dealer = await _dealerService.GetByEmailAsync(req.Email);

        if (dealer is null || !BCrypt.Net.BCrypt.Verify(req.Password, dealer.Password))
        {
            ThrowError("Invalid email or password.", 401);
            return;
        }

        var token = JwtBearer.CreateToken(jwt =>
        {
            jwt.SigningKey = _configuration["Jwt:SigningKey"]!;
            jwt.ExpireAt = DateTime.Now.AddHours(4);
            jwt.User.Claims.Add(("DealerId", dealer.Id.ToString()));
            jwt.User.Claims.Add(("Email", dealer.Email));
        });

        await Send.OkAsync(new LoginResponse { DealerId = dealer.Id, Email = dealer.Email, Token = token }, ct);
    }
}