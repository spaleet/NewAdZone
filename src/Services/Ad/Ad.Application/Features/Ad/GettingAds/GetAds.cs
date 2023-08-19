using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Models.Paging;

namespace Ad.Application.Features.Ad.GettingAds;

public record GetAds : PagingQuery<GetAdsResponse>;


