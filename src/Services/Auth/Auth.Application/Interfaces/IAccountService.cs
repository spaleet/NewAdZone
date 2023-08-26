using Auth.Application.Models;

namespace Auth.Application.Interfaces;
public interface IAccountService
{
    Task<UserProfileDto> GetUserProfile(string id);
}
