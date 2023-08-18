using BuildingBlocks.Core.Exceptions.Base;

namespace Ad.Application.Features.AdGallery.GettingGallery;

public record GetGallery(Guid Id) : IQuery<GetGalleryResponse>;

public class GetGalleryValidator : AbstractValidator<GetGallery>
{
    public GetGalleryValidator()
    {
        RuleFor(x => x.Id)
            .RequiredValidator("شناسه");
    }
}

public class GetGalleryHandler : IQueryHandler<GetGallery, GetGalleryResponse>
{
    private readonly IAdDbContext _context;

    public GetGalleryHandler(IAdDbContext context)
    {
        _context = context;
    }

    public async Task<GetGalleryResponse> Handle(GetGallery request, CancellationToken cancellationToken)
    {
        var gallery = await _context.AdGalleries.FindAsync(request.Id);

        if (gallery is null)
            throw new NotFoundException("گالری پیدا نشد");

        string path = Path.Combine(AdPathConsts.Gallery, gallery.ImageSrc);

        return new GetGalleryResponse(path, gallery.ContentType);
    }
}