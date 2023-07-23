using Ad.Application.Exceptions;
using Ad.Application.Interfaces;
using AutoMapper;
using BuildingBlocks.Core.CQRS.Commands;
using FluentValidation;
using MediatR;

namespace Ad.Application.Features.AdCategory.CreatingAdCategory;

public record CreateAdCategory(string Title, long? ParentId) : ICommand;

public class CreateAdCategoryValidator : AbstractValidator<CreateAdCategory>
{
    public CreateAdCategoryValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("عنوان را وارد کنید");
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
        var category = _mapper.Map(request, new Domain.Entities.AdCategory());

        if (request.ParentId != null || request.ParentId != 0)
        {
            var parent = await _context.AdCategories.FindAsync(request.ParentId);

            if (parent is null)
                throw new InvalidParentException();
            else
                category.ParentId = parent.Id;
        }

        await _context.AdCategories.AddAsync(category);
        await _context.SaveChangesAsync();

        return Unit.Value;
    }
}