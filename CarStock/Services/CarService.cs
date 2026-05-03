using Dapper;
using CarStock.Models;
using CarStock.Interfaces;
using CarStock.Helpers.Exceptions;
using Microsoft.Data.Sqlite;

namespace CarStock.Services;

public class CarService : ICarService
{
    private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

    public CarService(IDatabaseConnectionFactory databaseConnectionFactory)
    {
        _databaseConnectionFactory = databaseConnectionFactory;
    }

    // Add Car
    public async Task<int> InsertAsync(Car car)
    {
        using var connection = _databaseConnectionFactory.CreateConnection();

        try
        {
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
        catch (SqliteException ex) when (ex.SqliteErrorCode == 19)
        {
            throw new AddException("DUPLICATE_CAR", "This car already exists for this dealer.");
        }
    }

    // Get car
    public async Task<Car?> GetByIdAsync(int dealerId, int carId)
    {
        using var connection = _databaseConnectionFactory.CreateConnection();
        var car = await connection.QuerySingleOrDefaultAsync<Car>(@"
            SELECT id, dealer_id, make, model, year, stock, created, updated
            FROM cars
            WHERE id = @Id
            AND dealer_id = @DealerId",
            new { Id = carId, DealerId = dealerId }
        );

        if (car == null)
            throw new AddException("NOT_FOUND", "Car not found.");

        return car;
    }

    // List cars and stock levels
    public async Task<IEnumerable<Car>> GetDealerStockAsync(int dealerId)
    {
        using var connection = _databaseConnectionFactory.CreateConnection();
        return await connection.QueryAsync<Car>(@"
            SELECT id, dealer_id, make, model, year, stock, created, updated
            FROM cars
            WHERE dealer_id = @DealerId
            ORDER BY make, model, year",
            new { DealerId = dealerId }
        );
    }

    // Search car by make and model
    public async Task<IEnumerable<Car>> SearchAsync(int dealerId, string? make, string? model)
    {
        using var connection = _databaseConnectionFactory.CreateConnection();
        return await connection.QueryAsync<Car>(@"
            SELECT id, dealer_id, make, model, year, stock, created, updated
            FROM cars
            WHERE dealer_id = @DealerId
            AND (@Make IS NULL OR make LIKE '%' || @Make || '%')
            AND (@Model IS NULL OR model LIKE '%' || @Model || '%')
            ORDER BY make, model, year",
            new { DealerId = dealerId, Make = make, Model = model }
        );
    }

    // Update car stock level
    public async Task<bool> UpdateAsync(int dealerId, int carId, int stock)
    {
        using var connection = _databaseConnectionFactory.CreateConnection();
        var rows = await connection.ExecuteAsync(@"
            UPDATE cars
            SET stock = @Stock, updated = @Updated
            WHERE id = @Id
            AND dealer_Id = @DealerId",
            new
            {
                Stock = stock,
                Id = carId,
                DealerId = dealerId,
                Updated = DateTime.Now.ToString("G")
            }
        );

        if (rows == 0)
            throw new AddException("NOT_FOUND", "Car not found.");

        return true;
    }

    // Remove car
    public async Task<bool> DeleteAsync(int dealerId, int carId)
    {
        using var connection = _databaseConnectionFactory.CreateConnection();

        var rows = await connection.ExecuteAsync(@"
            DELETE FROM cars
            WHERE id = @Id
            AND dealer_id = @DealerId",
            new { Id = carId, @DealerId = dealerId }
        );

        if (rows == 0)
            throw new AddException("NOT_FOUND", "Car not found.");

        return true;
    }
}