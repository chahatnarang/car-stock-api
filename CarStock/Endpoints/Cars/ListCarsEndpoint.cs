using CarStock.Interfaces;
using FastEndpoints;

namespace CarStock.Endpoints.Cars;

public class ListCarsEndpoint : EndpointWithoutRequest<IEnumerable<CarResponse>>
{
    private readonly ICarService _carService;
    private readonly ICurrerntDealerService _currentDealerService;

    public ListCarsEndpoint(ICarService carService, ICurrerntDealerService currerntDealerService)
    {
        _carService = carService;
        _currentDealerService = currerntDealerService;
    }

    public override void Configure()
    {
        Get("/cars");
        Description(d => d.WithTags("Cars"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var dealerId = _currentDealerService.GetDealerId();
        var cars = await _carService.GetDealerStockAsync(dealerId);

        var response = cars.Select(car => new CarResponse
        {
           Id = car.Id,
           Make = car.Make,
           Model = car.Model,
           Year = car.Year,
           Stock = car.Stock 
        });

        await Send.OkAsync(response, ct);
    }
}