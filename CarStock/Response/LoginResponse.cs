namespace CarStock.Response;

public class LoginResponse
{
    public int DealerId {get; init;}
    public string Email {get; init;} = "";
    public string Token {get; init;} = "";
}