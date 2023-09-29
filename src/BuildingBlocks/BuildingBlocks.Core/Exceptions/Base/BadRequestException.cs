using System.Net;

namespace BuildingBlocks.Core.Exceptions.Base;

public class BadRequestException : CustomException
{
    public BadRequestException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
}