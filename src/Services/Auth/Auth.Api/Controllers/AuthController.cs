﻿using System.Security.Claims;
using System.Text.Json;
using Auth.Application.Interfaces;
using Auth.Application.Models;
using BuildingBlocks.Core.Web;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class AuthController : BaseControllerLite
{
    private readonly IAuthTokenStoreService _tokenStoreService;
    private readonly IAuthUserService _userService;

    public AuthController(IAuthUserService userService, IAuthTokenStoreService tokenStoreService)
    {
        _userService = userService;
        _tokenStoreService = tokenStoreService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountRequest model)
    {
        await _userService.RegisterAsync(model);

        return Ok("User created successfully.");
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn([FromBody] AuthenticateUserRequest login)
    {
        var authenticateResult = await _userService.AuthenticateUserAsync(login);

        string result = JsonSerializer.Serialize(authenticateResult, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return Ok(result);
    }

    [HttpPost("sign-out")]
    public async Task<IActionResult> SignOut([FromBody] RevokeRefreshTokenRequest token)
    {
        var claimsIdentity = User.Identity as ClaimsIdentity;
        string? userId = claimsIdentity?.FindFirst(ClaimTypes.UserData)?.Value;

        await _tokenStoreService.RevokeUserBearerTokens(Guid.Parse(userId), token.RefreshToken);

        return Ok("You've successfully Signed Out!");
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RevokeRefreshTokenRequest token)
    {
        var refreshTokenResult = await _userService.RevokeTokenAsync(token);

        string result = JsonSerializer.Serialize(refreshTokenResult, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        return Ok(result);
    }

    [HttpPost("is-authenticated")]
    public IActionResult IsAuthenticated()
    {
        if (!User.Identity.IsAuthenticated)
            return Unauthorized();

        return Ok("You're successfully Authorized!");
    }

    // TODO : Activate Email?
}
