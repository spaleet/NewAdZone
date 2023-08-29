using System.Text.Json.Serialization;
using BuildingBlocks.Core.Utilities.ImageRelated;

namespace Auth.Application.Models;
public record RegisterAccountRequest
{
    public string Username { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string Password { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}

public class RegisterAccountRequestValidator : AbstractValidator<RegisterAccountRequest>
{
    public RegisterAccountRequestValidator()
    {
        RuleFor(x => x.Username)
            .RequiredValidator("نام کاربری");

        RuleFor(x => x.PhoneNumber)
            .RequiredValidator("ایمیل");
        
        RuleFor(x => x.Password)
            .RequiredValidator("رمز عبور");

        RuleFor(x => x.FirstName)
            .RequiredValidator("نام");

        RuleFor(x => x.LastName)
            .RequiredValidator("نام خانوادگی");
    }
}