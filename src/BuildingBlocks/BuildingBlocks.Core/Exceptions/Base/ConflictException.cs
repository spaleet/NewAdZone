using System.Net;

namespace BuildingBlocks.Core.Exceptions.Base;

public class ConflictException : CustomException
{
    public ConflictException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.Conflict;
    }
}