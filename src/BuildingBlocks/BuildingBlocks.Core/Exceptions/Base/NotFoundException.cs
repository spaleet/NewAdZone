using System.Net;

namespace BuildingBlocks.Core.Exceptions.Base;

public class NotFoundException : CustomException
{
    public NotFoundException(string message)
        : base(message)
    {
        StatusCode = HttpStatusCode.NotFound;
    }
}