using Dapper;


public sealed class CarRepository : ICarService
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
    public Task<Car?> GetByIdAsync(int dealerId, int carId)
    {
        throw new NotImplementedException();
    }

    // List cars and stock levels
    public Task<IEnumerable<Car>> GetDealerStockAsync(int dealerId)
    {
        throw new NotImplementedException();
    }

    // Search car by make and model
    public Task<IEnumerable<Car>> SearchAsync(int dealerId, string? make, string? model)
    {
        throw new NotImplementedException();
    }

    // Update car stock level
    public Task<bool> UpdateAsync(int dealerId, int carId, int stock)
    {
        throw new NotImplementedException();
    }

    // Remove car
      public Task<bool> DeleteAsync(int dealerId, int carId)
    {
        throw new NotImplementedException();
    }
}