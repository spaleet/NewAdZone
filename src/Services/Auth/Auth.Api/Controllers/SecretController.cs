using Auth.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class SecretController : BaseController
{
    [Authorize]
    [HttpGet("all")]
    public IActionResult All()
    {
        return Ok("You can access all secret!");
    }

    [Authorize(Policy = nameof(Roles.BasicUser))]
    [HttpGet("user")]
    public IActionResult UserSecret()
    {
        return Ok("You can access user secret!");
    }

    [Authorize(Policy = nameof(Roles.VerifiedUser))]
    [HttpGet("verified-user")]
    public IActionResult VerifiedUserSecret()
    {
        return Ok("You can access verified user secret!");
    }

    [Authorize(Policy = nameof(Roles.Admin))]
    [HttpGet("admin")]
    public IActionResult AdminSecret()
    {
        return Ok("You can access admin secret!");
    }
}