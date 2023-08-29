namespace Auth.Application.Models;

public record ChangePasswordRequest
{
    public string Id { get; set; }

    public string CurrentPassword { get; set; }

    public string NewPassword { get; set; }
}

public class ChangePasswordRequestValidator : AbstractValidator<ChangePasswordRequest>
{
    public ChangePasswordRequestValidator()
    {
        RuleFor(x => x.Id)
            .RequiredValidator("شناسه");

        RuleFor(x => x.CurrentPassword)
            .RequiredValidator("رمز عبور قبل");

        RuleFor(x => x.NewPassword)
            .RequiredValidator("رمز عبور جدید");
    }
}