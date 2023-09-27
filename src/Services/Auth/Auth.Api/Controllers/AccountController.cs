using Auth.Application.Interfaces;
using Auth.Application.Models;
using Auth.Domain.Enums;
using BuildingBlocks.Core.Web;
using BuildingBlocks.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

[Authorize(Policy = AuthConsts.BasicUser)]
public class AccountController : BaseControllerLite
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("profile/{id}")]
    public async Task<ActionResult<UserProfileDto>> GetProfile([FromRoute] string id)
    {
        var res = await _accountService.GetUserProfile(id);

        return Ok(res);
    }

    [HttpPut("edit")]
    public async Task<IActionResult> EditProfile([FromForm] EditProfileRequest req)
    {
        await _accountService.EditUserProfile(req);

        return Ok("حساب شما با موفقیت ویرایش شد!");
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> EditProfile([FromBody] ChangePasswordRequest req)
    {
        await _accountService.ChangePassword(req);

        return Ok("رمز با موفقیت تغییر یافت!");
    }
}
