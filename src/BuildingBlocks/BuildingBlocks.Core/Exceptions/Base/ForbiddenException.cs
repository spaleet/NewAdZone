using System.Net;

namespace BuildingBlocks.Core.Exceptions.Base;

public class ForbiddenException : IdentityException
{
    public ForbiddenException(string message)
        : base(message, statusCode: HttpStatusCode.Forbidden) { }
}