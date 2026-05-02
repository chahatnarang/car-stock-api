using CarStock.Models;

public interface IDealerService
{
    Task<Dealer?> GetByEmailAsync(string email);
}