using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuildingBlocks.Security.Interfaces;
using BuildingBlocks.Security.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Security.Services;

public class JwtTokenFactory : IJwtTokenFactory
{
    private readonly ILogger<JwtTokenFactory> _logger;
    private readonly BearerTokenSettings _tokenSettings;

    public JwtTokenFactory(IOptions<BearerTokenSettings> tokenSettings, ILogger<JwtTokenFactory> logger)
    {
        _tokenSettings = tokenSettings.Value;
        _logger = logger;
    }


    public async Task<JwtTokenResponse> CreateJwtTokenAsync(string userId, string userName, string email, string serialNumber, IReadOnlyList<string>? rolesClaims)
    {
        var (accessToken, claims) = await CreateAccessTokenAsync(user);

        var (refreshTokenValue, refreshTokenSerial) = CreateRefreshToken();

        return new JwtTokenResponse(
            accessToken,
            refreshTokenValue,
            refreshTokenValue,
            claims);
    }

    public string? GetRefreshTokenSerial(string refreshTokenValue)
    {
        if (string.IsNullOrWhiteSpace(refreshTokenValue))
            return string.Empty;

        var decodedRefreshTokenPrincipal = new ClaimsPrincipal();
        try
        {
            decodedRefreshTokenPrincipal = new JwtSecurityTokenHandler().ValidateToken(
                refreshTokenValue,
                new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret)),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                },
                out _
            );
        }
        catch (Exception ex)
        {
            _logger.LogError("Failed to validate RefreshToken Value : {refreshTokenValue}. ERROR : {ex}", ex,
                refreshTokenValue);
        }

        return decodedRefreshTokenPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.SerialNumber)?.Value;
    }


}
