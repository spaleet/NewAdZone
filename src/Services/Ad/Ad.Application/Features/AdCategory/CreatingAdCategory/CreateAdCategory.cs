using AutoMapper;

namespace Ad.Application.Features.AdCategory.CreatingAdCategory;

public record CreateAdCategory(string Title, long? ParentId) : ICommand;

public class CreateAdCategoryValidator : AbstractValidator<CreateAdCategory>
{
    public CreateAdCategoryValidator()
    {
        RuleFor(x => x.Title)
            .RequiredValidator("عنوان");
    }
}

public class CreateAdCategoryHandler : ICommandHandler<CreateAdCategory>
{
    private readonly IAdDbContext _context;
    private readonly IMapper _mapper;

    public CreateAdCategoryHandler(IAdDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateAdCategory request, CancellationToken cancellationToken)
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

        return Unit.Value;
    }
}