using System.Security.Claims;
using BuildingBlocks.Security.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BuildingBlocks.Security.Services;
public class AuthTokenValidatorService : IAuthTokenValidatorService
{
    //private readonly IAuthTokenStoreService _authTokenStoreService;
    //private readonly UserManager<User> _userManager;

    //public AuthTokenValidatorService(UserManager<User> userManager, IAuthTokenStoreService authTokenStoreService)
    //{
    //    _userManager = userManager;
    //    _authTokenStoreService = authTokenStoreService;
    //}

    public async Task ValidateAsync(TokenValidatedContext context)
    {
        var claimsIdentity = context.Principal?.Identity as ClaimsIdentity;

        if (claimsIdentity?.Claims is null || !claimsIdentity.Claims.Any())
        {
            context.Fail("This is not our issued token. It has no claims.");
            return;
        }

        var serialNumberClaim = claimsIdentity.FindFirst(ClaimTypes.SerialNumber);
        if (serialNumberClaim is null)
        {
            context.Fail("This is not our issued token. It has no serial.");
            return;
        }

        string userId = claimsIdentity.FindFirst(ClaimTypes.UserData)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            context.Fail("This is not our issued token. It has no user-id.");
            return;
        }

        //var user = await _userManager.FindByIdAsync(userId);

        //if (user is null || user.SerialNumber != serialNumberClaim.Value)
        //{
        //    context.Fail("This token is expired. Please login again.");
        //    return;
        //}


        //if (context.SecurityToken is not JwtSecurityToken accessToken ||
        //    string.IsNullOrWhiteSpace(accessToken.RawData) ||
        //    !await _authTokenStoreService.IsValidToken(accessToken.RawData, Guid.Parse(userId)))
        //{
        //    context.Fail("This token is not in our database.");
        //    return;
        //}
    }
}
