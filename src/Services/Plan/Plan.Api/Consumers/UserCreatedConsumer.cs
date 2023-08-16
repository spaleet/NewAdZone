using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Plan.Application.Features.GettingPlans;
using Plan.Application.Features.SubscribingPlan;
using Plan.Domain.Enums;

namespace Plan.Api.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<UserCreatedConsumer> _logger;

    public UserCreatedConsumer(IMediator mediator, ILogger<UserCreatedConsumer> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var userId = context.Message.UserId;
        _logger.LogInformation("Found user by {userId}", userId);



        var plans = await _mediator.Send(new GetPlans());

        string planId = plans.Plans.FirstOrDefault(x => x.Title == nameof(PlanNameConsts.BasicPlan)).Id;

        _logger.LogInformation("Found plan by {id}", planId);

        await _mediator.Send(new SubscribePlan(planId, userId));
    }
}
