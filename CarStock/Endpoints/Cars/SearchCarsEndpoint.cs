using FastEndpoints;
using FluentValidation;
using CarStock.Interfaces;

namespace CarStock.Endpoints.Cars;

public class SearchCarRequest
{
    public string Make { get; init; } = "";
    public string Model { get; init; } = "";
}

public class SearchCarValidator : Validator<SearchCarRequest>
{
    public SearchCarValidator()
    {
        RuleFor(rule => rule)
            .Must(rule => !string.IsNullOrWhiteSpace(rule.Make) || !string.IsNullOrWhiteSpace(rule.Model))
            .WithMessage("Make or model cannot be empty");
    }

    public class SearchCarEndpoint : Endpoint<SearchCarRequest, IEnumerable<CarResponse>>
    {
        private readonly ICarService _carService;
        private readonly ICurrerntDealerService _currentDealerService;

        public SearchCarEndpoint(ICarService carService, ICurrerntDealerService currentDealerService)
        {
            _carService = carService;
            _currentDealerService = currentDealerService;
        }

        public override void Configure()
        {
            Get("cars/search");
            Description(d => d.WithTags("Cars"));
        }

        public override async Task HandleAsync(SearchCarRequest req, CancellationToken ct)
        {
            var dealerId = _currentDealerService.GetDealerId();
            var cars = await _carService.SearchAsync(dealerId, req.Make, req.Model);

            var response = cars.Select(c => new CarResponse
            {
                Id = c.Id,
                Make = c.Make,
                Model = c.Model,
                Year = c.Year,
                Stock = c.Stock
            });

            await Send.OkAsync(response, ct);
        }
    }
}