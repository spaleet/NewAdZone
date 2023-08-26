using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;
using Plan.Application.Features.CheckingUserLimit;

namespace Plan.Api.Controllers;

public class UserPlanController : BaseController
{
    [HttpGet("verify")]
    public async Task<IActionResult> VerifiedUser([FromQuery(Name = "uid")] string uid)
    {
        bool res = await Mediator.Send(new CheckUserLimit(uid));

        return res ? Ok("با موفقیت اشتراک دارید!") : BadRequest("اشتراک فعالی ندارید!");
    }
}
