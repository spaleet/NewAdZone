using Ad.Application.Dtos;
using AutoMapper;

namespace Ad.Application.Features.Ad.GettingRelatedAds;

public record GetRelatedAds(string Slug) : IQuery<List<AdDto>>;

public class GetRelatedAdsValidator : AbstractValidator<GetRelatedAds>
{
    public GetRelatedAdsValidator()
    {
        RuleFor(x => x.Slug)
            .RequiredValidator("آدرس اسلاگ");
    }
}

public class GetRelatedAdsHandler : IQueryHandler<GetRelatedAds, List<AdDto>>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public GetRelatedAdsHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<AdDto>> Handle(GetRelatedAds request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .IgnoreAutoIncludes()
            .Select(x => new
            {
                x.Slug,
                x.CategoryId
            })
            .FirstOrDefaultAsync(x => x.Slug == request.Slug);

        // check for null
        AdNotFoundException.ThrowIfNull(ad);

        long categoryId = ad.CategoryId;

        var relatedAds = await _context.Ads.IgnoreAutoIncludes()
                                           .OrderByDescending(x => x.LastUpdateDate)
                                           .Where(x => x.CategoryId == categoryId)
                                           .Take(5)
                                           .Select(x => _mapper.Map(x, new AdDto()))
                                           .ToListAsync();

        return relatedAds;
    }
}