using BuildingBlocks.Core.Web;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plan.Application.Features.GettingPlans;
using Plan.Infrastructure.Context;

namespace Plan.Api.Controllers;

public class PlanController : BaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetPlans(CancellationToken cancellationToken)
    {
        var plans = await Mediator.Send(new GetPlans(), cancellationToken);

        return Ok(plans);
    }
}
