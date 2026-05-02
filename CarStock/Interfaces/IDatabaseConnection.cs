using Microsoft.Data.Sqlite;

public interface IDatabaseConnection
{
    SqliteConnection CreateConnection();
}