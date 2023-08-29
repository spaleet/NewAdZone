using MongoDB.Driver;

namespace Plan.Application.Features.CheckingUserLimit;

public record CheckUserLimit(string UserId, int UsedQuota) : IQuery<bool>;

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

        if (planSub is null) return false;

        var plan = _context.Plans.Find(x => x.Id == planSub.Id).FirstOrDefault();

        if (plan is null) return false;

        if (plan.MonthlyQuota <= request.UsedQuota)
            return false;

        return true;
    }
}