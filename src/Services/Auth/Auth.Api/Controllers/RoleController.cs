using Auth.Application.Interfaces;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class RoleController : BaseControllerLite
{
    private readonly IAuthUserService _userService;
    public RoleController(IAuthUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("verify")]
    public async Task<IActionResult> VerifiedUser([FromQuery(Name = "uid")] string uid)
    {
        bool res = await _userService.IsVerifiedRole(uid);

        return res ? Ok() : Forbid();
    }
}
