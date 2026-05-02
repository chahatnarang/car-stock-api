using Microsoft.Data.Sqlite;

namespace CarStock.Interfaces;

public interface IDatabaseConnection
{
    SqliteConnection CreateConnection();
}