namespace Auth.Application.Models;
public class RevokeRefreshTokenRequest
{
    public string RefreshToken { get; set; }
}

public class RevokeRefreshTokenRequestValidator : AbstractValidator<RevokeRefreshTokenRequest>
{
    public RevokeRefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .RequiredValidator("توکن");
    }
}