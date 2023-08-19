using Ad.Application.Dtos;
using AutoMapper;
using BuildingBlocks.Core.Models.Paging;

namespace Ad.Application.Features.Ad.GettingAds;

public record GetAds(string? Search) : PagingQuery<GetAdsResponse>;

public class GetAdsValidator : AbstractValidator<GetAds>
{
    public GetAdsValidator()
    {
    }
}

public class GetAdsHandler : IQueryHandler<GetAds, GetAdsResponse>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public GetAdsHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetAdsResponse> Handle(GetAds request, CancellationToken cancellationToken)
    {
        var query = _context.Ads.IgnoreAutoIncludes().OrderByDescending(x => x.CreateDate)
                                             .AsNoTracking().AsQueryable();

        // search by title
        if (!string.IsNullOrEmpty(request.Search))
            query = query.Where(x => x.Title.Contains(request.Search) || x.Tags.Contains(request.Search));

        // apply paging
        var ads = await query.ApplyPagingAsync<Domain.Entities.Ad, AdDto>(
                                                                          _mapper.ConfigurationProvider,
                                                                          request.Page,
                                                                          request.PageSize);

        return new GetAdsResponse(ads);
    }
}