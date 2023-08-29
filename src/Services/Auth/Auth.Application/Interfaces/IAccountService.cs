using Auth.Application.Models;

namespace Auth.Application.Interfaces;
public interface IAccountService
{
    Task<UserProfileDto> GetUserProfile(string id);

    Task EditUserProfile(EditProfileRequest model);

    Task ChangePassword(ChangePasswordRequest model);
}
