using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Auth.Application.Interfaces;
public interface IAuthTokenValidatorService
{
    Task ValidateAsync(TokenValidatedContext context);
}
