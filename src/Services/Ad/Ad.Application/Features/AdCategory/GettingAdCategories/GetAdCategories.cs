using Ad.Application.Dtos;
using AutoMapper;

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
        var categories = await _context.AdCategories.AsQueryable()
                                                    .Select(x => _mapper.Map(x, new AdCategoryDto()))
                                                    .ToListAsync();

        var parents = categories.Where(x => x.ParentId == null).ToList();

        var response = new List<AdCategoryOrderedResponse>();

        foreach (var p in parents)
        {
            var children = categories.Where(x => x.ParentId == p.Id).ToList();
            response.Add(new AdCategoryOrderedResponse(p, children));
        }

        return new GetAdCategoriesResponse(response);
    }
}