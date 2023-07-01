using System.Text;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Plan.Application.Features.CreatingNewPlan;
using Plan.Application.Features.GettingPlans;
using Plan.Application.Features.InitializingPayment;
using Plan.Application.Features.SubscribingPlan;

namespace Plan.Api.Controllers;

public class PlanController : BaseController
{
    [HttpGet("")]
    public async Task<IActionResult> GetPlans(CancellationToken cancellationToken)
    {
        var plans = await Mediator.Send(new GetPlans(), cancellationToken);

        return Ok(plans);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreatePlan([FromBody] CreateNewPlan request, CancellationToken cancellationToken)
    {
        await Mediator.Send(request, cancellationToken);

        return Ok("Plan successfully created!");
    }

    [HttpPost("subscribe")]
    public async Task<IActionResult> SubscribeToPlan([FromBody] SubscribePlan request, CancellationToken cancellationToken)
    {
        var res = await Mediator.Send(request, cancellationToken);

        return Ok(res);
    }

    [HttpPost("initialize")]
    public async Task<IActionResult> InitializePayment([FromQuery] string subId, [FromQuery] decimal price,
        [FromQuery] string callBack)
    {
        subId = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(subId));

        var request = new InitializePayment(subId, price, callBack);

        var res = await Mediator.Send(request);

        return Ok(res);
    }
}
