using System.Text.Json.Serialization;
using BuildingBlocks.Core.Validation;

namespace Auth.Application.Models;
public record AuthenticateUserRequest
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; }
}

public class AuthenticateUserRequestValidator : AbstractValidator<AuthenticateUserRequest>
{
    public AuthenticateUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .CustomEmailAddressValidator();

        RuleFor(x => x.Password)
            .RequiredValidator("رمز عبور");
    }
}