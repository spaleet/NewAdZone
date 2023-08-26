using Auth.Application.Interfaces;
using Auth.Application.Models;
using AutoMapper;
using BuildingBlocks.Core.Exceptions.Base;
using Microsoft.Extensions.Logging;

namespace Auth.Application.Services;

public class AccountService : IAccountService
{
    private readonly ILogger<AccountService> _log;
    private readonly UserManager<User> _userManager;
    private readonly IMapper _mapper;

    public AccountService(UserManager<User> userManager, IMapper mapper, ILogger<AccountService> log)
    {
        _userManager = userManager;
        _mapper = mapper;
        _log = log;
    }

    public async Task<UserProfileDto> GetUserProfile(string id)
    {
        var user = await _userManager.FindByIdAsync(id);

        if (user is null)
            throw new NotFoundException("User not found!");

        var mappedUser = _mapper.Map<UserProfileDto>(user);

        var roles = await _userManager.GetRolesAsync(user);
        mappedUser.Roles = roles.ToArray();

        return mappedUser;
    }
}
