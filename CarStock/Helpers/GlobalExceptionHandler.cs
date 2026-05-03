using CarStock.Helpers.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace CarStock.Helpers;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken ct)
    {
        if (exception is not AddException ex) return false;

        var (statusCode, message) = ex.ErrorCode switch
        {
            "NOT_FOUND" => (404, ex.Message),
            "DUPLICATE_CAR" => (409, ex.Message),
            _ => (400, ex.Message)
        };

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(new { StatusCode = statusCode, Message = message, ErrorCode = ex.ErrorCode }, ct);
        return true;
    }
}