using Microsoft.Data.Sqlite;

namespace CarStock.Interfaces;

public interface IDatabaseConnectionFactory
{
    SqliteConnection CreateConnection();
}