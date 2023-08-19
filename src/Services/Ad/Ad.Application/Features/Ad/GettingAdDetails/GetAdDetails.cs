using AutoMapper;

namespace Ad.Application.Features.Ad.GettingAd;

public record GetAdDetails(string Slug) : IQuery<GetAdDetailsResponse>;

public class GetAdValidator : AbstractValidator<GetAdDetails>
{
    public GetAdValidator()
    {
        RuleFor(x => x.Slug)
            .RequiredValidator("آدرس اسلاگ");
    }
}


public class GetAdDetailsHandler : IQueryHandler<GetAdDetails, GetAdDetailsResponse>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public GetAdDetailsHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAdDetailsResponse> Handle(GetAdDetails request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AdCategory)
            .Include(x => x.AdGalleries)
            .FirstOrDefaultAsync(x => x.Slug == request.Slug);

        // check for null
        AdNotFoundException.ThrowIfNull(ad);

        return _mapper.Map<GetAdDetailsResponse>(ad);
    }
}