using System.Text.Json.Serialization;

namespace Auth.Application.Models;
public record AuthenticateUserRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}