using BuildingBlocks.Messaging.Events;
using MassTransit;
using MediatR;
using Plan.Application.Features.SubscribingPlan;

namespace Plan.Api.Consumers;

public class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
    private readonly IMediator _mediator;

    public UserCreatedConsumer(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task Consume(ConsumeContext<UserCreatedEvent> context)
    {
        var userId = context.Message.UserId;



        await _mediator.Send(new SubscribePlan("sdsd", userId));
    }
}
