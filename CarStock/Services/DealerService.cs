using Dapper;
using CarStock.Models;
using CarStock.Interfaces;

namespace CarStock.Services;

public class DealerService : IDealerService
{
    private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

    public DealerService(IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _databaseConnectionFactory = databaseConnectionFactory;
    }

    public async Task<Dealer?> GetByEmailAsync(string email)
    {
        using var connection = _databaseConnectionFactory.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Dealer>(@"
            SELECT id, name, email, password, created
            FROM dealers
            WHERE email = @Email",
            new { Email = email }
        );
    }
}