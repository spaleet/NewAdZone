using BuildingBlocks.Security.Models;

namespace BuildingBlocks.Security.Interfaces;

public interface IJwtTokenFactory
{
    Task<JwtTokenResponse> CreateJwtTokenAsync(
        string userId,
        string userName,
        string email,
        string serialNumber,
        IReadOnlyList<string>? rolesClaims);

    string? GetRefreshTokenSerial(string refreshTokenValue);
}