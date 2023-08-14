using Ad.Application.Consts;
using Ad.Application.Interfaces;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Exceptions.Base;
using FluentValidation;

namespace Ad.Application.Features.AdGallery.GettingGallery;

public record GetGallery(Guid Id) : IQuery<GetGalleryResponse>;

public class GetGalleryValidator : AbstractValidator<GetGallery>
{
    public GetGalleryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("لطفا شناسه وارد کنید");
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