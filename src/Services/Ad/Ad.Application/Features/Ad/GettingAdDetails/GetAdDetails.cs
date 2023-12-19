using AutoMapper;
using BuildingBlocks.Cache;

namespace Ad.Application.Features.Ad.GettingAd;

public record GetAdDetails(string Slug) : IQuery<GetAdDetailsResponse>;

public class GetAdDetailsValidator : AbstractValidator<GetAdDetails>
{
    public GetAdDetailsValidator()
    {
        RuleFor(x => x.Slug)
            .RequiredValidator("آدرس اسلاگ");
    }
}

public class GetAdDetailsHandler : IQueryHandler<GetAdDetails, GetAdDetailsResponse>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public GetAdDetailsHandler(IAdDbContext context, IMapper mapper, ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<GetAdDetailsResponse> Handle(GetAdDetails request, CancellationToken cancellationToken)
    {
        string cacheKey = $"{CacheKeyConsts.Ad}_{request.Slug}";

        return await _cacheService.Get<GetAdDetailsResponse>(cacheKey, async () =>
        {
            var ad = await _context.Ads
            .Include(x => x.AdCategory)
            .Include(x => x.AdGalleries)
            .FirstOrDefaultAsync(x => x.Slug == request.Slug);

            // check for null
            AdNotFoundException.ThrowIfNull(ad);

            return _mapper.Map<GetAdDetailsResponse>(ad);

        }, TimeSpan.FromMinutes(2));
    }
}