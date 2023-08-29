using BuildingBlocks.Core.Utilities.ImageRelated;
using Microsoft.AspNetCore.Http;

namespace Auth.Application.Models;
public record EditProfileRequest
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Bio { get; set; }

    public IFormFile? AvatarSource { get; set; }
}

public class EditProfileRequestValidator : AbstractValidator<EditProfileRequest>
{
    public EditProfileRequestValidator()
    {
        RuleFor(x => x.Id)
            .RequiredValidator("شناسه");

        RuleFor(x => x.Username)
            .RequiredValidator("نام کاربری");

        RuleFor(x => x.PhoneNumber)
            .RequiredValidator("ایمیل");

        RuleFor(x => x.FirstName)
            .RequiredValidator("نام");

        RuleFor(x => x.LastName)
            .RequiredValidator("نام خانوادگی");

        RuleFor(x => x.Bio)
            .RequiredValidator("بیو")
            .MinLengthValidator("بیو", 25)
            .MaxLengthValidator("بیو", 500);

        RuleFor(x => x.AvatarSource)
            .MaxFileSizeValidator(MaxFileSize.Megabyte(3), false);
    }
}