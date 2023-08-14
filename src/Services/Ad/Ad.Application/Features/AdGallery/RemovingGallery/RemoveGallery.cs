using BuildingBlocks.Core.Exceptions.Base;

namespace Ad.Application.Features.AdGallery.RemovingGallery;

public record RemoveGallery(Guid Id) : ICommand;

public class RemoveGalleryValidator : AbstractValidator<RemoveGallery>
{
    public RemoveGalleryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("لطفا شناسه وارد کنید");
    }
}

public class RemoveGalleryHandler : ICommandHandler<RemoveGallery>
{
    private readonly IAdDbContext _context;

    public RemoveGalleryHandler(IAdDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(RemoveGallery request, CancellationToken cancellationToken)
    {
        var gallery = await _context.AdGalleries.FindAsync(request.Id);

        if (gallery is null)
            throw new NotFoundException("گالری پیدا نشد");

        ImageHelper.DeleteImage(gallery.ImageSrc, AdPathConsts.Gallery);

        _context.AdGalleries.Remove(gallery);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}