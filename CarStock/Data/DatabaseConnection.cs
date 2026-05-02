using CarStock.Interfaces;
using Microsoft.Data.Sqlite;

public class DatabaseConnection : IDatabaseConnection
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