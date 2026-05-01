using Dapper;
using Microsoft.Data.Sqlite;

namespace car_stock.Data;

public static class DatabaseInitialiser
{
    public static void Initialise(string connectionString)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        connection.Execute(@"
            CREATE TABLE IF NOT TABLE EXISTS dealers (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                name TEXT NOT NULL,
                email TEXT NOT NULL UNIQUE,
                password TEXT NOT NULL,
                created TEXT NOT NULL
            )
        ");

        connection.Execute(@"
            CREATE TABLE IF NOT TABLE EXISTS cars (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                dealer_id INTEGER NOT NULL,
                make TEXT NOT NULL,
                model TEXT NOT NULL UNIQUE,
                year INTEGER NOT NULL,
                stock INTEGER NOT NULL DEFAULT 0,
                created TEXT NOT NULL,
                updated TEXT NOT NULL
                FOREIGN KEY (dealer_id) REFERENCES dealer(id)
            )
        ");

        connection.Execute(@"CREATE INDEX IF NOT EXISTS idx_cars_dealer_id ON cars(dealer_id)");
        connection.Execute(@"CREATE INDEX IF NOT EXISTS idx_cars_dealer_make_model ON cars(dealer_id, make, model)");

        var dealerCount = connection.ExecuteScalar<int>("SELECT COUNT(*) FROM dealers");
        if (dealerCount == 0)
        {
            var utcNow = DateTime.UtcNow;
            var aestZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            var aestNow = TimeZoneInfo.ConvertTimeFromUtc(utcNow, aestZone);
            var now = aestNow.ToString("o");

            connection.Execute(@"
                INSERT INTO dealers (name, email, password, created)
                VALUES (@NName, @Email, @Password, @Created)",
                new
                {
                    Name = "Cars Melbourne",
                    Email = "cars@carsmelbourne.com.au",
                    Password = BCrypt.Net.BCrypt.HashPassword("MelbournePassword123!"),
                    Created = now
                }
            );

            connection.Execute(@"
                INSERT INTO dealers (name, email, password, created)
                VALUES (@NName, @Email, @Password, @Created)",
                new
                {
                    Name = "Cars Footscray",
                    Email = "cars@carsfootscray.com.au",
                    Password = BCrypt.Net.BCrypt.HashPassword("FootscrayPassword123!"),
                    Created = now
                }
            );
        }
    }
}