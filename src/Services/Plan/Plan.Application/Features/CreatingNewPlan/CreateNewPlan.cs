using AutoMapper;
using BuildingBlocks.Core.CQRS.Commands;
using FluentValidation;
using MediatR;
using Plan.Infrastructure.Context;

namespace Plan.Application.Features.CreatingNewPlan;

public record CreateNewPlan(string Title, int AdQuota, decimal? Price) : ICommand;

public class CreateNewPlanValidator : AbstractValidator<CreateNewPlan>
{
    public CreateNewPlanValidator()
    {
        RuleFor(x => x.Title).NotEmpty().WithMessage("عنوان را وارد کنید");
        RuleFor(x => x.AdQuota)
            .NotEmpty().NotNull().WithMessage("کوتا را وارد کنید")
            .NotEqual(0).WithMessage("کوتا معتبر وارد کنید");

        RuleFor(x => x.Price)
            .Must(x =>
            {
                if (x is not null)
                    if (x < 0 || x == 0) 
                        return false;

                return true;
            }).WithMessage("قیمت معتبر وارد کنید");
    }
}

public class CreateNewPlanHandler : ICommandHandler<CreateNewPlan>
{
    private readonly PlanDbContext _context;
    private readonly IMapper _mapper;

    public CreateNewPlanHandler(PlanDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Unit> Handle(CreateNewPlan request, CancellationToken cancellationToken)
    {
        var plan = _mapper.Map(request, new Domain.Entities.Plan());

        await _context.Plans.InsertOneAsync(plan);

        return Unit.Value;
    }
}