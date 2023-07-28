using Ad.Application.Dtos;
using Ad.Application.Interfaces;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Queries;
using BuildingBlocks.Core.Exceptions.Base;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Ad.Application.Features.Ad.GettingAd;

public record GetAd(long Id) : IQuery<AdDto>;

public class GetAdValidator : AbstractValidator<GetAd>
{
    public GetAdValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("شناسه را وارد کنید");
    }
}


public class GetAdHandler : IQueryHandler<GetAd, AdDto>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public GetAdHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<AdDto> Handle(GetAd request, CancellationToken cancellationToken)
    {
        var ad = await _context.Ads
            .Include(x => x.AdCategory)
            .Include(x => x.AdGalleries)
            .FirstOrDefaultAsync(x => x.Id == request.Id);

        if (ad is null) throw new NotFoundException("آگهی مورد نظر پیدا نشد");

        return _mapper.Map<AdDto>(ad);
    }
}