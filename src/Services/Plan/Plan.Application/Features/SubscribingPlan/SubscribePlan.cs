using BuildingBlocks.Core.Utilities;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Plan.Application.Features.SubscribingPlan;

public record SubscribePlan : ICommand<SubscribePlanResponse>
{
    public string PlanId { get; set; }

    [JsonIgnore]
    [BindNever]
    public string UserId { get; set; }
}

public class SubscribePlanValidator : AbstractValidator<SubscribePlan>
{
    public SubscribePlanValidator()
    {
        RuleFor(x => x.PlanId)
            .RequiredValidator("شناسه پلن");

        RuleFor(x => x.UserId)
            .RequiredValidator("شناسه کاربر");
    }
}

public class SubscribePlanHandler : ICommandHandler<SubscribePlan, SubscribePlanResponse>
{
    private readonly PlanDbContext _context;

    public SubscribePlanHandler(PlanDbContext context)
    {
        _context = context;
    }

    public async Task<SubscribePlanResponse> Handle(SubscribePlan request, CancellationToken cancellationToken)
    {
        var plan = _context.Plans.AsQueryable().FirstOrDefault(x => x.Id == request.PlanId);

        if (plan is null)
            throw new NotFoundException("پلن مورد نظر پیدا نشد");

        // TODO : check user exists

        var userWithPlan = _context.PlanSubscriptions.AsQueryable()
            .Where(x => x.UserId == request.UserId && x.PlanId == request.PlanId)
            .FirstOrDefault();

        if (userWithPlan != null)
            throw new BadRequestException("شما قبلا این پلن را انتخاب کرده اید");

        var subscription = new PlanSubscription
        {
            PlanId = request.PlanId,
            UserId = request.UserId,
            State = PlanSubscriptionState.Pending,
            Price = plan.Price,
            IssueTrackingNo = "0000-0000"
        };

        if (plan.Price == 0)
        {
            subscription.State = PlanSubscriptionState.Subscribed;
            subscription.SubscriptionStart = DateTime.Now;
            subscription.SubscriptionExpire = DateTime.Now.AddMonths(1);
            subscription.IssueTrackingNo = Generator.IssueTrackingCode();
        }

        await _context.PlanSubscriptions.InsertOneAsync(subscription);

        return new SubscribePlanResponse(subscription.Id, subscription.Price);
    }
}