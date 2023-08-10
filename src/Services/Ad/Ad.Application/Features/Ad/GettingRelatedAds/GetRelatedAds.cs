using Ad.Application.Dtos;
using Ad.Application.Features.Ad.GettingAd;
using Ad.Application.Interfaces;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Exceptions.Base;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Ad.Application.Features.Ad.GettingRelatedAds;

public record GetRelatedAds(string Slug) : IQuery<List<AdDto>>;

public class GetRelatedAdsValidator : AbstractValidator<GetRelatedAds>
{
    public GetRelatedAdsValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("آدرس اسلاگ را وارد کنید");
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
            .FirstOrDefaultAsync(x => x.Slug == request.Slug);

        //TODO only select cat id

        if (ad is null) throw new NotFoundException("آگهی مورد نظر پیدا نشد");

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