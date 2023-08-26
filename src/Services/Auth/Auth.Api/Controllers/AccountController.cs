using Auth.Application.Interfaces;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class AccountController : BaseControllerLite
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("profile/{id}")]
    public async Task<IActionResult> GetProfile([FromRoute] string id)
    {
        var res = await _accountService.GetUserProfile(id);

        return Ok(res);
    }
}
