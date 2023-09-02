using Auth.Application.Consts;
using Auth.Application.Interfaces;
using Auth.Application.Models;
using AutoMapper;
using BuildingBlocks.Core.Exceptions.Base;
using BuildingBlocks.Core.Utilities.ImageRelated;
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

    public async Task EditUserProfile(EditProfileRequest model)
    {
        // validation
        EditProfileRequestValidator validator = new();
        validator.ValidateWithResponse(model);

        var user = await _userManager.FindByIdAsync(model.Id);

        if (user is null)
            throw new NotFoundException("User not found!");

        _mapper.Map(model, user);

        // update image
        if (model.AvatarSource is not null)
        {
            if (user.Avatar != AuthPathConsts.DefaultAvatar)
            {
                // delete prevoius image
                ImageHelper.DeleteImage(user.Avatar, AuthPathConsts.Avatar);
            }

            // upload new image
            string uploadFileName = model.AvatarSource.UploadImage(AuthPathConsts.Avatar, width: 500, height: 500);

            user.Avatar = uploadFileName;
        }

        var res = await _userManager.UpdateAsync(user);

        if (!res.Succeeded)
            throw new BadRequestException(res.Errors.FirstOrDefault().Description);

    }

    public async Task ChangePassword(ChangePasswordRequest model)
    {
        // validation
        ChangePasswordRequestValidator validator = new();
        validator.ValidateWithResponse(model);

        var user = await _userManager.FindByIdAsync(model.Id);

        if (user is null)
            throw new NotFoundException("User not found!");

        var res = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!res.Succeeded)
            throw new BadRequestException(res.Errors.FirstOrDefault().Description);
    }
}
