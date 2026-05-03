using Dapper;
using Microsoft.Data.Sqlite;

public static class DatabaseInitialiser
{
    public static void Initialise(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS dealers (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                email TEXT NOT NULL UNIQUE,
                password TEXT NOT NULL,
                created TEXT NOT NULL
            )
        ");

        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS cars (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                dealer_id INTEGER NOT NULL,
                make TEXT NOT NULL,
                model TEXT NOT NULL UNIQUE,
                year INTEGER NOT NULL,
                stock INTEGER NOT NULL DEFAULT 0,
                created TEXT NOT NULL,
                updated TEXT NOT NULL,
                FOREIGN KEY (dealer_id) REFERENCES dealers(id)
            )
        ");

        connection.Execute(@"CREATE INDEX IF NOT EXISTS idx_cars_dealer_id ON cars(dealer_id)");
        connection.Execute(@"CREATE INDEX IF NOT EXISTS idx_cars_dealer_make_model ON cars(dealer_id, make, model)");

        var dealerCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM dealers");
        if (dealerCount == 0)
        {
            connection.Execute(@"
                INSERT INTO dealers (name, email, password, created)
                VALUES (@Name, @Email, @Password, @Created)",
                new
                {
                    Name = "Cars Melbourne",
                    Email = "dealer@carsmelbourne.com.au",
                    Password = BCrypt.Net.BCrypt.HashPassword("MelbournePassword123!"),
                    Created = DateTime.Now.ToString("G")
                }
            );

            connection.Execute(@"
                INSERT INTO dealers (name, email, password, created)
                VALUES (@Name, @Email, @Password, @Created)",
                new
                {
                    Name = "Cars Footscray",
                    Email = "dealer@carsfootscray.com.au",
                    Password = BCrypt.Net.BCrypt.HashPassword("FootscrayPassword123!"),
                    Created = DateTime.Now.ToString("G")
                }
            );
        }
    }
}