using Auth.Application.Models;

namespace Auth.Application.Interfaces;
public interface IAuthUserService
{
    Task<string> RegisterAsync(RegisterAccountRequest model);
    Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest model);
    Task<AuthenticateUserResponse> RevokeTokenAsync(RevokeRefreshTokenRequest model);
}
