using CarStock.Interfaces;
using FastEndpoints;
using FluentValidation;

namespace CarStock.Endpoints.Cars;

public class UpdateStockRequest
{
    public int Id { get; init; }
    public int Stock { get; init; }
}

public class UpdateCarStockValidator : Validator<UpdateStockRequest>
{
    public UpdateCarStockValidator()
    {
        RuleFor(stock => stock.Stock).GreaterThanOrEqualTo(0);
    }
}

public class UpdateStockEndpoint : Endpoint<UpdateStockRequest>
{
    private readonly ICarService _carService;
    private readonly ICurrerntDealerService _currentDealerService;

    public UpdateStockEndpoint(ICarService carService, ICurrerntDealerService currerntDealerService)
    {
        _carService = carService;
        _currentDealerService = currerntDealerService;
    }

    public override void Configure()
    {
        Put("/cars/{id}/stock");
        Description(d => d.WithTags("Cars"));
    }

    public override async Task HandleAsync(UpdateStockRequest req, CancellationToken ct)
    {
        var dealerId = _currentDealerService.GetDealerId();
        var updated = await _carService.UpdateAsync(dealerId, req.Id, req.Stock);

        if (!updated)
        {
            await Send.NotFoundAsync();
            return;
        }

        await Send.NoContentAsync(ct);
    }
}