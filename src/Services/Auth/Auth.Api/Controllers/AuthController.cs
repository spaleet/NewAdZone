using System.Security.Claims;
using Auth.Application.Interfaces;
using Auth.Application.Models;
using BuildingBlocks.Core.Web;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers;

public class AuthController : BaseControllerLite
{
    private readonly IAuthTokenStoreService _tokenStoreService;
    private readonly IAuthUserService _userService;
    private readonly IPublishEndpoint _publisher;

    public AuthController(IAuthUserService userService, IAuthTokenStoreService tokenStoreService, IPublishEndpoint publisher)
    {
        _userService = userService;
        _tokenStoreService = tokenStoreService;
        _publisher = publisher;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterAccountRequest model)
    {
        string userId = await _userService.RegisterAsync(model);

        // publish user registered event
        var @event = new UserCreatedEvent(userId);

        await _publisher.Publish<UserCreatedEvent>(@event);

        return Ok("User created successfully.");
    }

    [HttpPost("sign-in")]
    public async Task<ActionResult<AuthenticateUserResponse>> SignIn([FromBody] AuthenticateUserRequest login)
    {
        var authenticateResult = await _userService.AuthenticateUserAsync(login);

        return Ok(authenticateResult);
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
    public async Task<ActionResult<AuthenticateUserResponse>> RefreshToken([FromBody] RevokeRefreshTokenRequest token)
    {
        var refreshTokenResult = await _userService.RevokeTokenAsync(token);

        return Ok(refreshTokenResult);
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
