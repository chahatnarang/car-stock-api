using Dapper;
using CarStock.Models;
using CarStock.Interfaces;

public class CarRepository : ICarService
{
    private readonly DatabaseConnection _databaseConnection;

    public CarRepository(DatabaseConnection databaseConnection)
    {
        _databaseConnection = databaseConnection;
    }

    // Add Car
    public async Task<int> InsertAsync(Car car)
    {
        using var connection = _databaseConnection.CreateConnection();

        return await connection.ExecuteScalarAsync<int>(@"
            INSERT INTO cars (dealer_id, make, model, year, stock, created, updated)
            VALUES (@DealerId, @Make, @Model, @Year, @Stock, @Created, @Updated);
            SELECT last_insert_rowid();",
            new
            {
                car.DealerId,
                car.Make,
                car.Model,
                car.Year,
                car.Stock,
                Created = DateTime.Now.ToString("G"),
                Updated = DateTime.Now.ToString("G")
            });
    }

    // Get car
    public async Task<Car?> GetByIdAsync(int dealerId, int carId)
    {
        using var connection = _databaseConnection.CreateConnection();

        return await connection.QuerySingleOrDefaultAsync<Car>(@"
            SELECT
                id,
                dealer_id,
                make,
                model,
                year,
                stock,
                created,
                updated
            FROM cars
            WHERE id = @Id
            AND dealer_id = @DealerId",
            new { Id = carId, DealerId = dealerId }
        );
    }

    // List cars and stock levels
    public async Task<IEnumerable<Car>> GetDealerStockAsync(int dealerId)
    {
        using var connection = _databaseConnection.CreateConnection();
        return await connection.QueryAsync<Car>(@"
            SELECT
                id,
                dealer_id,
                make,
                model,
                year,
                stock,
                created,
                updated
            FROM cars
            WHERE dealer_id = @DealerId
            ORDERBY make, model, year",
            new { DealerId = dealerId }
        );
    }

    // Search car by make and model
    public async Task<IEnumerable<Car>> SearchAsync(int dealerId, string? make, string? model)
    {
        using var connection = _databaseConnection.CreateConnection();
        return await connection.QueryAsync<Car>(@"
            SELECT
                id,
                dealer_id,
                make,
                model,
                year,
                stock,
                created,
                updated
            FROM cars
            WHERE dealer_id = @DealerId
            AND (@Make IS NULL OR make LIKE '%' || @Make || '%')
            AND (@Model IS NULL OR make LIKE '%' || @Model || '%')
            ORDERBY make, model, year",
            new { DealerId = dealerId, Make = make, Model = model }
        );
    }

    // Update car stock level
    public async Task<bool> UpdateAsync(int dealerId, int carId, int stock)
    {
        using var connection = _databaseConnection.CreateConnection();
        var rows = await connection.ExecuteAsync(@"
            UPDATE cars
            SET stock = @Stock,
                updated = @Updated
            WHERE id = @Id
            AND dealer_Id = @DealerId",
            new
            {
                Stock = stock,
                Updated = DateTime.Now.ToString("G"),
                Id = carId,
                DealerId = dealerId
            }    
        );
        return rows > 0;
    }

    // Remove car
    public async Task<bool> DeleteAsync(int dealerId, int carId)
    {
        using var connection = _databaseConnection.CreateConnection();

        var rows = await connection.ExecuteAsync(@"
            DELETE FROM cars
            WHERE id = @Id
            AND dealer_id = @DealerId",
            new { Id = carId, @DelearId = dealerId }
        );
        return rows > 0;
    }
}