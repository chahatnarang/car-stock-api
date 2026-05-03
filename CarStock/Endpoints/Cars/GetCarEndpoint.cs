using FastEndpoints;
using CarStock.Interfaces;

namespace CarStock.Endpoints.Cars;

public class GetCarRequest
{
    public int Id { get; init; }
}

public class GetCarEndpoint : Endpoint<GetCarRequest, CarResponse>
{
    private readonly ICarService _carService;
    private readonly ICurrentDealerService _currentDealerService;

    public GetCarEndpoint(ICarService carService, ICurrentDealerService currerntDealerService)
    {
        _carService = carService;
        _currentDealerService = currerntDealerService;
    }

    public override void Configure()
    {
        Get("cars/{id}");
        Description(d => d.WithTags("Cars"));
    }

    public override async Task HandleAsync(GetCarRequest req, CancellationToken ct)
    {
        var dealerId = _currentDealerService.GetDealerId();
        var car = await _carService.GetByIdAsync(dealerId, req.Id);

        if (car is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(new CarResponse
        {
            Id = car.Id,
            Make = car.Make,
            Model = car.Model,
            Year = car.Year,
            Stock = car.Stock
        });
    }

}