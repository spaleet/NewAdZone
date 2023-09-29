using System.Net;

namespace BuildingBlocks.Core.Exceptions.Base;

public class UnAuthorizedException : IdentityException
{
    public UnAuthorizedException(string message)
        : base(message, HttpStatusCode.Unauthorized) { }
}