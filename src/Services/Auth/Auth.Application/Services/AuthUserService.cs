using Auth.Application.Exceptions;
using Auth.Application.Interfaces;
using Auth.Application.Models;
using Auth.Domain.Enums;
using AutoMapper;
using BuildingBlocks.Core.Exceptions.Base;

namespace Auth.Application.Services;
public class AuthUserService : IAuthUserService
{
    private readonly SignInManager<User> _signInManager;
    private readonly IJwtTokenFactory _jwtTokenFactory;
    private readonly IAuthTokenStoreService _authTokenStore;
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;

    public AuthUserService(UserManager<User> userManager, SignInManager<User> signInManager,
        IJwtTokenFactory jwtTokenFactory, IAuthTokenStoreService authTokenStore, IMapper mapper)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtTokenFactory = jwtTokenFactory;
        _authTokenStore = authTokenStore;
        _mapper = mapper;
    }

    public async Task<string> RegisterAsync(RegisterAccountRequest model)
    {
        // Validate Email
        InvalidEmailException.ThrowIfNotValid(model.Email);

        // Duplicate Email
        var duplicateEmail = await _userManager.FindByEmailAsync(model.Email);
        if (duplicateEmail is not null)
            throw new InvalidEmailException(model.Email ?? string.Empty);

        // Validate Phone
        InvalidPhoneNumberException.ThrowIfNotValid(model.PhoneNumber);

        // Duplicate Phone
        var duplicatePhone = await _userManager.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);
        if (duplicatePhone is not null)
            throw new InvalidPhoneNumberException(model.PhoneNumber);

        var user = _mapper.Map<User>(model);
        user.Avatar = "default-avatar.png";

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
            throw new ApiException(result.Errors.First().Description);

        await _userManager.AddToRoleAsync(user, Roles.BasicUser.ToString());

        return user.Id.ToString();
    }

    public async Task<AuthenticateUserResponse> AuthenticateUserAsync(AuthenticateUserRequest model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);

        if (user is null)
            throw new NotFoundException("No user was found.");

        var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);

        if (!result.Succeeded)
            throw new UnAuthorizedException("Authentication failed.");

        var userRoles = (IReadOnlyList<string>)await _userManager.GetRolesAsync(user);

        var token = await _jwtTokenFactory.CreateJwtTokenAsync(user.Id.ToString(),
                                                               user.UserName,
                                                               user.Email,
                                                               user.SerialNumber,
                                                               userRoles);

        await _authTokenStore.AddUserToken(user, token.RefreshTokenSerial, token.AccessToken);

        return new AuthenticateUserResponse(token);
    }

    public async Task<AuthenticateUserResponse> RevokeTokenAsync(RevokeRefreshTokenRequest model)
    {
        if (string.IsNullOrWhiteSpace(model.RefreshToken))
            throw new InvalidTokenException();

        var token = await _authTokenStore.FindToken(model.RefreshToken);

        if (token == null)
            throw new InvalidTokenException();

        var user = await _userManager.FindByIdAsync(token.UserId.ToString());

        if (user is null)
            throw new NotFoundException("No user was found with the provided credentials.");

        var userRoles = (IReadOnlyList<string>)await _userManager.GetRolesAsync(user);

        var jwtResult = await _jwtTokenFactory.CreateJwtTokenAsync(user.Id.ToString(),
                                                               user.UserName,
                                                               user.Email,
                                                               user.SerialNumber,
                                                               userRoles);

        string? refreshTokenSerial = _jwtTokenFactory.GetRefreshTokenSerial(model.RefreshToken);

        await _authTokenStore.AddUserToken(user, jwtResult.RefreshTokenSerial, jwtResult.AccessToken,
            refreshTokenSerial);

        return new AuthenticateUserResponse(jwtResult);
    }
}
