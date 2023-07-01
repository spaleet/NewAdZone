namespace Plan.Application.Features.SubscribingPlan;

public record SubscribePlanResponse(string? SubscriptionId, SubscribePlanState State);

public enum SubscribePlanState
{
    Error,
    AlreadyHavePlan,
    Success
}
