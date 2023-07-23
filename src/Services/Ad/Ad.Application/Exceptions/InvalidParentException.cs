using BuildingBlocks.Core.Exceptions.Base;

namespace Ad.Application.Exceptions;

public class InvalidParentException : BadRequestException
{
    public InvalidParentException() : base("دسته بندی وارد شده معتبر نمی باشد")
    {
    }
}
