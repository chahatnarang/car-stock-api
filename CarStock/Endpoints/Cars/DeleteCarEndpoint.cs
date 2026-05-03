using CarStock.Interfaces;
using FastEndpoints;

namespace CarStock.Endpoints.Cars;

public class DeleteCarRequest()
{
    public int Id { get; init; }
}

public class DeleteCarEndpoint: Endpoint<DeleteCarRequest>
{
    public readonly ICarService _carService;
    public readonly ICurrentDealerService _currentDealerService;

    public DeleteCarEndpoint(ICarService carService, ICurrentDealerService currentDealerService)
    {
        _carService = carService;
        _currentDealerService = currentDealerService;
    }

    public override void Configure()
    {
        Delete("cars/{id}");
        Description(d => d.WithTags("Cars"));
    }

    public override async Task HandleAsync(DeleteCarRequest req, CancellationToken ct)
    {
        var dealerId = _currentDealerService.GetDealerId();
        var deleted = await _carService.DeleteAsync(dealerId, req.Id);

        if (!deleted)
        {
            await Send.NotFoundAsync();
            return;
        }

        await Send.NoContentAsync(ct);
    }
}