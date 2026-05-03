using FastEndpoints;
using CarStock.Models;
using FluentValidation;
using CarStock.Interfaces;

namespace CarStock.Endpoints.Cars;

public class AddCarRequest()
{
    public string Make { get; init; } = "";
    public string Model { get; init; } = "";
    public int Year { get; init; }
    public int Stock { get; init; }
}

public class AddCarValidator : Validator<AddCarRequest>
{
    public AddCarValidator()
    {
        RuleFor(rule => rule.Make).NotEmpty().MaximumLength(256);
        RuleFor(rule => rule.Model).NotEmpty().MaximumLength(256);
        RuleFor(rule => rule.Year).InclusiveBetween(1885, DateTime.Now.Year + 1);
        RuleFor(rule => rule.Stock).GreaterThanOrEqualTo(0);
    }

    public class AddCarEndpoint : Endpoint<AddCarRequest, CarResponse>
    {
        private readonly ICarService _carService;
        private readonly ICurrerntDealerService _currentDealerService;

        public AddCarEndpoint(ICarService carservice, ICurrerntDealerService currerntDealerService)
        {
            _carService = carservice;
            _currentDealerService = currerntDealerService;
        }

        public override void Configure()
        {
            Post("/cars");
            Description(d => d.WithTags("Cars"));
        }

        public override async Task HandleAsync(AddCarRequest req, CancellationToken ct)
        {
            var dealerId = _currentDealerService.GetDealerId();

            var car = new Car
            {
                DealerId = dealerId,
                Make = req.Make,
                Model = req.Model,
                Year = req.Year,
                Stock = req.Stock,
                Updated = DateTime.Now.ToString("G"),
                Created = DateTime.Now.ToString("G")
            };

            var newId = await _carService.InsertAsync(car);

            await Send.CreatedAtAsync<GetCarEndpoint>(
                routeValues: new { id = newId },
                responseBody: new CarResponse
                {
                    Id = newId,
                    Make = car.Make,
                    Model = car.Model,
                    Year = car.Year,
                    Stock = car.Stock
                });
        }
    }
}