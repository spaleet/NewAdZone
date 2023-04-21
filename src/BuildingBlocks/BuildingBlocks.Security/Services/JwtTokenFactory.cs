using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BuildingBlocks.Security.Interfaces;
using BuildingBlocks.Security.Models;
using BuildingBlocks.Security.Utils;
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


    public async Task<JwtTokenResponse> CreateJwtTokenAsync(string userId,
                                                            string userName,
                                                            string email,
                                                            string serialNumber,
                                                            IReadOnlyList<string>? rolesClaims)
    {
        var (accessToken, claims) = await CreateAccessTokenAsync(userId, userName, email, serialNumber, rolesClaims);

        var (refreshTokenValue, refreshTokenSerial) = await CreateRefreshToken();

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

    private Task<(string AccessToken, List<Claim> Claims)> CreateAccessTokenAsync(string userId,
                                                                                        string userName,
                                                                                        string email,
                                                                                        string serialNumber,
                                                                                        IReadOnlyList<string>? rolesClaims)
    {
        var now = DateTime.Now;
        var ipAddress = IpUtilities.GetIpAddress();

        var claims = new List<Claim>
        {
            // Unique Id for all Jwt tokes
            new(JwtRegisteredClaimNames.Jti, SecurityUtilities.CreateCryptographicallySecureGuid().ToString()),

            // Issuer
            new(JwtRegisteredClaimNames.Iss, _tokenSettings.Issuer),

            // Issued at
            new(JwtRegisteredClaimNames.Iat, now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)),

            // User Data
            new(ClaimTypes.NameIdentifier, userId),
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Email, email),
            new(ClaimTypes.SerialNumber, serialNumber),
            new(ClaimTypes.UserData, userId),
            new("ip", ipAddress),
        };

        // User Roles
        if (rolesClaims?.Any() is true)
        {
            foreach (string role in rolesClaims)
                claims.Add(new Claim(ClaimTypes.Role, role.ToLower(CultureInfo.InvariantCulture), ClaimValueTypes.String, _tokenSettings.Issuer)));
        }

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expireTime = now.AddMinutes(_tokenSettings.AccessTokenExpirationMinutes);

        var token = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audiance,
            claims: claims,
            notBefore: now,
            expires: expireTime,
            signingCredentials: signingCredentials);

        string accessToken = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult((accessToken, claims));
    }

    private Task<(string RefreshTokenValue, string RefreshTokenSerial)> CreateRefreshToken()
    {
        var now = DateTime.Now;

        string refreshTokenSerial = SecurityUtilities.CreateCryptographicallySecureGuid().ToString().Replace("-", "");

        var claims = new List<Claim>
        {
            // Unique Id for all Jwt tokes
            new(JwtRegisteredClaimNames.Jti, SecurityUtilities.CreateCryptographicallySecureGuid().ToString()),

            // Issuer
            new(JwtRegisteredClaimNames.Iss, _tokenSettings.Issuer),
            
            // Issued at
            new(JwtRegisteredClaimNames.Iat, now.ToString("yyyy-MM-ddTHH:mm:ssZ", CultureInfo.InvariantCulture)),

            // for invalidation
            new(ClaimTypes.SerialNumber, refreshTokenSerial)
        };

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _tokenSettings.Issuer,
            audience: _tokenSettings.Audiance,
            claims: claims,
            notBefore: now,
            expires: now.AddHours(_tokenSettings.RefreshTokenExpirationHours),
            signingCredentials: signingCredentials);

        string refreshTokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        return Task.FromResult((refreshTokenValue, refreshTokenSerial));
    }
}
