using Auth.Application.Interfaces;
using Auth.Domain.Enums;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class AccountController : BaseControllerLite
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [Authorize(Policy = nameof(Roles.BasicUser))]
    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetProfile([FromRoute] string id)
    {
        var res = await _accountService.GetUserProfile(id);

        return Ok(res);
    }
}
