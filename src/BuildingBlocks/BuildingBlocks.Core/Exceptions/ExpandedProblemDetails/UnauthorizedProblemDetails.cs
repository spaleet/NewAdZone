using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Core.Exceptions.ExpandedProblemDetails;

public class UnauthorizedProblemDetails : ProblemDetails
{
    public UnauthorizedProblemDetails(string? details = null)
    {
        Title = "UnauthorizedException";
        Detail = details;
        Status = 401;
        Type = "https://httpstatuses.com/401";
    }
}