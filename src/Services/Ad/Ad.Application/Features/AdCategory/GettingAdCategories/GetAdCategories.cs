using Ad.Application.Dtos;
using Ad.Application.Interfaces;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Queries;
using Microsoft.EntityFrameworkCore;

namespace Ad.Application.Features.AdCategory.GettingAdCategories;

public record GetAdCategories : IQuery<GetAdCategoriesResponse>;

public class GetAdCategoriesHandler : IQueryHandler<GetAdCategories, GetAdCategoriesResponse>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public GetAdCategoriesHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;

    }

    public async Task<GetAdCategoriesResponse> Handle(GetAdCategories request, CancellationToken cancellationToken)
    {
        var categories = await _context.AdCategories.AsQueryable().ToListAsync();

        var mappedCategories = categories.Select(x => _mapper.Map(x, new AdCategoryDto())).ToList();

        return new GetAdCategoriesResponse(mappedCategories);
    }
}
