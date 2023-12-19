using Ad.Application.Dtos;
using AutoMapper;
using BuildingBlocks.Cache;

namespace Ad.Application.Features.AdCategory.CreatingAdCategory;

public record CreateAdCategory(string Title, long? ParentId) : ICommand<AdCategoryDto>;

public class CreateAdCategoryValidator : AbstractValidator<CreateAdCategory>
{
    public CreateAdCategoryValidator()
    {
        RuleFor(x => x.Title)
            .RequiredValidator("عنوان");
    }
}

public class CreateAdCategoryHandler : ICommandHandler<CreateAdCategory, AdCategoryDto>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;

    public CreateAdCategoryHandler(IAdDbContext context, IMapper mapper, ICacheService cacheService)
    {
        _context = context;
        _mapper = mapper;
        _cacheService = cacheService;
    }

    public async Task<AdCategoryDto> Handle(CreateAdCategory request, CancellationToken cancellationToken)
    {
        // map request to AdCategory
        // make slug from provided title
        var category = _mapper.Map(request, new Domain.Entities.AdCategory());

        if (request.ParentId != null && request.ParentId != 0)
        {
            var parent = await _context.AdCategories.FindAsync(request.ParentId);

            if (parent is null)
                throw new InvalidParentException();
            else
                category.ParentId = parent.Id;
        }

        if (request.ParentId == 0) category.ParentId = null;

        await _context.AdCategories.AddAsync(category);
        await _context.SaveChangesAsync();

        await _cacheService.Remove(CacheKeyConsts.Categories);

        return _mapper.Map<AdCategoryDto>(category);
    }
}