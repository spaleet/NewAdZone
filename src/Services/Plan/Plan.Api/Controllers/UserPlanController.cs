using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Plan.Application.Features.CheckingUserLimit;

namespace Plan.Api.Controllers;

public class UserPlanController : BaseController
{
    [HttpGet("verify")]
    [Authorize]
    public async Task<IActionResult> VerifiedUser([FromQuery(Name = "uid")] string uid, [FromQuery(Name = "usedQuota")] int usedQuota)
    {
        bool res = await Mediator.Send(new CheckUserLimit(uid, usedQuota));

        return res ? Ok("با موفقیت اشتراک دارید!") : BadRequest("اشتراک فعالی ندارید!");
    }
}
