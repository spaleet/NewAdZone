using Auth.Domain.Enums;
using BuildingBlocks.Core.Web;
using BuildingBlocks.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class SecretController : BaseControllerLite
{
    [Authorize]
    [HttpGet("all")]
    public IActionResult All()
    {
        return Ok("You can access all secret!");
    }

    [Authorize(Policy = AuthConsts.BasicUser)]
    [HttpGet("user")]
    public IActionResult UserSecret()
    {
        return Ok("You can access user secret!");
    }

    [Authorize(Policy = AuthConsts.VerifiedUser)]
    [HttpGet("verified-user")]
    public IActionResult VerifiedUserSecret()
    {
        return Ok("You can access verified user secret!");
    }

    [Authorize(Policy = AuthConsts.Admin)]
    [HttpGet("admin")]
    public IActionResult AdminSecret()
    {
        return Ok("You can access admin secret!");
    }
}