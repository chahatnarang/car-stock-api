using Microsoft.Data.Sqlite;

namespace car_stock.Data;

public interface IDatabaseConnection
{
    SqliteConnection CreateConnection();
}
public sealed class DatabaseConnection : IDatabaseConnection
{
    private readonly string _connectionString;

    public DatabaseConnection(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("DefaultConnection not found or is missing.");
    }

    public SqliteConnection CreateConnection()
    {
        return new SqliteConnection(_connectionString);
    }
}