using Ad.Application.Dtos;
using Ad.Application.Features.Ad.GettingRelatedAds;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Models.Paging;

namespace Ad.Application.Features.Ad.GettingAds;

public record GetAds : PagingQuery<GetAdsResponse>;

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
        var ads = await _context.Ads.IgnoreAutoIncludes()
                .OrderByDescending(x => x.CreateDate)
                .AsNoTracking()
                .ApplyPagingAsync<Domain.Entities.Ad, AdDto>(_mapper.ConfigurationProvider, request.Page, request.PageSize);

        return new GetAdsResponse(ads);
    }
}