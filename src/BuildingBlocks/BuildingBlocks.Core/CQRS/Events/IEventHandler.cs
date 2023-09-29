using MediatR;

namespace BuildingBlocks.Core.CQRS.Events;

public interface IEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : INotification
{ }