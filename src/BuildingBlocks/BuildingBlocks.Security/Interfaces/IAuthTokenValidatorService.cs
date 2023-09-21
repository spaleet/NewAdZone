using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BuildingBlocks.Security.Interfaces;
public interface IAuthTokenValidatorService
{
    Task ValidateAsync(TokenValidatedContext context);
}
