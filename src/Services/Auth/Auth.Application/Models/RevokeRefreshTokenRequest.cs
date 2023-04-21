using System.Text.Json.Serialization;

namespace Auth.Application.Models;
public class RevokeRefreshTokenRequest
{
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = default!;
}
