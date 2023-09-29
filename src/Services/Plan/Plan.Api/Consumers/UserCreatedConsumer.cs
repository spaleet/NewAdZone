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
        string userId = context.Message.UserId;

        _logger.LogInformation("Message Recieved: UserCreatedEvent, UId : {userId}", userId);

        var plans = await _mediator.Send(new GetPlans());

        // subscribe to BasicPlan
        string planId = plans.Plans.FirstOrDefault(x => x.Title == nameof(PlanNameConsts.BasicPlan)).Id;

        await _mediator.Send(new SubscribePlan
        {
            PlanId = planId,
            UserId = userId
        });
    }
}