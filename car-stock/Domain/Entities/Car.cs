namespace car_stock.Domain.Entities;

public sealed class Car()
{
    public int Id { get; init; }
    public int DealerId { get; init; }
    public string Make { get; init; } = "";
    public string Model { get; init; } = "";
    public int Year { get; init; }
    public int Stock { get; init; }
    public string Created { get; init; } = "";
    public string Updated { get; init; } = "";
}
