using Dapper;
using CarStock.Models;
using CarStock.Interfaces;

public class DealerService : IDealerService
{
    private readonly IDatabaseConnection _databaseConnection;

    public DealerService(IDatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    public async Task<Dealer?> GetByEmailAsync(string email)
    {
        using var connection = _databaseConnection.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Dealer>(@"
            SELECT id, name, email, password, created FROM dealers WHERE email = @EMAIL",
            new { Email = email }
        );
    }
}