using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Abstractions;

public sealed class Problem(string error) : IResult
{
    public Task ExecuteAsync(HttpContext httpContext)
    {
        ProblemDetails problemDetails = new()
        {
            Title = "An error occurred while processing your request.",
            Detail = error,
            Instance = httpContext.Request.Path,
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://httpstatuses.com/500",
            Extensions = { ["traceId"] = httpContext.TraceIdentifier }
        };

        return httpContext.Response.WriteAsJsonAsync(problemDetails);
    }
}