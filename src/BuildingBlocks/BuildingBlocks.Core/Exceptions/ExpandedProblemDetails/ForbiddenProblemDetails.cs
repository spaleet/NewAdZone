using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Core.Exceptions.ExpandedProblemDetails;

public class ForbiddenProblemDetails : ProblemDetails
{
    public ForbiddenProblemDetails(string? details = null)
    {
        Title = "ForbiddenException";
        Detail = details;
        Status = 403;
        Type = "https://httpstatuses.com/403";
    }
}

