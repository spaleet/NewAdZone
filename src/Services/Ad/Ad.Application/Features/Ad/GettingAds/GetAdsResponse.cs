using Ad.Application.Dtos;
using BuildingBlocks.Core.Models.Paging;

namespace Ad.Application.Features.Ad.GettingAds;

public record GetAdsResponse(PagingModel<AdDto> Ads);
