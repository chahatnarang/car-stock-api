using CarStock.Interfaces;

namespace CarStock.Services;

public class CurrentDealerService : ICurrentDealerService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentDealerService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetDealerId()
    {
        var claim = _httpContextAccessor.HttpContext?.User.FindFirst("DealerId")?.Value;

        if (claim is null || !int.TryParse(claim, out var dealerId))
            throw new UnauthorizedAccessException("DealerId claim missing or invalid.");

        return dealerId;
    }
}