using BuildingBlocks.Core.Exceptions.Base;

namespace Ad.Application.Exceptions;

public class AdNotFoundException : NotFoundException
{
    public AdNotFoundException() : base("آگهی مورد نظر پیدا نشد")
    {
    }

    public static void ThrowIfNull(object? ad)
    {
        if(ad is null)
            throw new AdNotFoundException();
    }
}
