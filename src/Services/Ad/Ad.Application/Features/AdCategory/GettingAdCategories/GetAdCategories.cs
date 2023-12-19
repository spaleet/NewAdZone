using Ad.Application.Dtos;
using AutoMapper;
using BuildingBlocks.Cache;

namespace Ad.Application.Features.AdCategory.GettingAdCategories;

public record GetAdCategories : IQuery<GetAdCategoriesResponse>;

public class GetAdCategoriesHandler : IQueryHandler<GetAdCategories, GetAdCategoriesResponse>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public GetAdCategoriesHandler(IAdDbContext context, IMapper mapper, ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<GetAdCategoriesResponse> Handle(GetAdCategories request, CancellationToken cancellationToken)
    {
        var categories = await _cacheService.Get<List<AdCategoryDto>>("categories");

        if (categories is null)
        {
            categories = await _context.AdCategories.AsQueryable()
                                                        .Select(x => _mapper.Map(x, new AdCategoryDto()))
                                                        .ToListAsync();
        }

        var parents = categories.Where(x => x.ParentId == null).ToList();

        var response = new List<AdCategoryOrderedResponse>();

        foreach (var p in parents)
        {
            var children = categories.Where(x => x.ParentId == p.Id).ToList();
            response.Add(new AdCategoryOrderedResponse(p, children));
        }

        await _cacheService.Set<List<AdCategoryDto>>("categories", categories);

        return new GetAdCategoriesResponse(response);
    }
}