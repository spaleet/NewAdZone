using MediatR;
using Microsoft.AspNetCore.Mvc;
using Plan.Application.Features.GettingPlans;
using Plan.Infrastructure.Context;

namespace Plan.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanController : ControllerBase
{
    private readonly PlanDbContext _ctx;
    private readonly IMediator _mediator;

    public PlanController(IMediator mediator, PlanDbContext ctx)
    {
        _mediator = mediator;
        _ctx = ctx;
    }

    [HttpGet("")]
    public async Task<IActionResult> GetPlans(CancellationToken cancellationToken)
    {
        var plans = await _mediator.Send(new GetPlans(), cancellationToken);

        return Ok(plans);
    }
}
