public interface ICarService
{
    Task<int> InsertAsync(Car car);
    Task<Car?> GetByIdAsync(int dealerId, int carId);
    Task<IEnumerable<Car>> GetDealerStockAsync(int dealerId);
    Task<bool> UpdateAsync(int dealerId, int carId, int stock);
    Task<bool> DeleteAsync(int dealerId, int carId);
    Task<IEnumerable<Car>> SearchAsync(int dealerId, string? make, string? model);
}