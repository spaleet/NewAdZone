using AutoMapper;

namespace Ad.Application.Features.Ad.GettingAd;

public record GetAd(string Slug) : IQuery<GetAdResponse>;

public class GetAdValidator : AbstractValidator<GetAd>
{
    public GetAdValidator()
    {
        RuleFor(x => x.Slug)
            .RequiredValidator("آدرس اسلاگ");
    }
}


public class GetAdHandler : IQueryHandler<GetAd, GetAdResponse>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public GetAdHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAdResponse> Handle(GetAd request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AdCategory)
            .Include(x => x.AdGalleries)
            .FirstOrDefaultAsync(x => x.Slug == request.Slug);

        // check for null
        AdNotFoundException.ThrowIfNull(ad);

        return _mapper.Map<GetAdResponse>(ad);
    }
}