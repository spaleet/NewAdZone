using BuildingBlocks.Core.Exceptions.Base;

namespace Auth.Application.Exceptions;

public class InvalidTokenException : BadRequestException
{
    public InvalidTokenException() : base("Token is invalid!")
    {
    }
}