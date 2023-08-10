using Ad.Application.Dtos;
using Ad.Application.Interfaces;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Exceptions.Base;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Ad.Application.Features.Ad.GettingAd;

public record GetAd(string Slug) : IQuery<GetAdResponse>;

public class GetAdValidator : AbstractValidator<GetAd>
{
    public GetAdValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty()
            .WithMessage("آدرس اسلاگ را وارد کنید");
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

        if (ad is null) throw new NotFoundException("آگهی مورد نظر پیدا نشد");

        return _mapper.Map<GetAdResponse>(ad);
    }
}