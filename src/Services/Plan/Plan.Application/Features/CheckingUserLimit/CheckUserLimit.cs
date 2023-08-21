using MongoDB.Driver;

namespace Plan.Application.Features.CheckingUserLimit;

public record CheckUserLimit(string UserId) : IQuery<bool>;

public class CheckUserLimitValidator : AbstractValidator<CheckUserLimit>
{
    public CheckUserLimitValidator()
    {
        RuleFor(x => x.UserId)
            .RequiredValidator("شناسه");
    }
}

public class CheckUserLimitHandler : IQueryHandler<CheckUserLimit, bool>
{
    private readonly PlanDbContext _context;
    public CheckUserLimitHandler(PlanDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(CheckUserLimit request, CancellationToken cancellationToken)
    {
        var planSub = _context.PlanSubscriptions.Find(x => x.UserId == request.UserId)
            .FirstOrDefault();

        return planSub is not null;
    }
}