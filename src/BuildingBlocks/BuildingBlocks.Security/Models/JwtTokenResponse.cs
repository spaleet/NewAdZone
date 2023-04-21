using System.Security.Claims;

namespace BuildingBlocks.Security.Models;

public record JwtTokenResponse(string AccessToken, string RefreshToken, string RefreshTokenSerial, List<Claim> Claims);
