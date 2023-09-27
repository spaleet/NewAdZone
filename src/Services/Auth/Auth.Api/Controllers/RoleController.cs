using Auth.Application.Interfaces;
using BuildingBlocks.Core.Web;
using BuildingBlocks.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Authorize(Policy = AuthConsts.BasicUser)]
public class RoleController : BaseControllerLite
{
    private readonly IAuthUserService _userService;
    public RoleController(IAuthUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("verify")]
    public async Task<IActionResult> VerifiedUser([FromQuery(Name = "uid")] string uid)
    {
        bool res = await _userService.IsVerifiedRole(uid);

        return res ? Ok() : Forbid();
    }
}
