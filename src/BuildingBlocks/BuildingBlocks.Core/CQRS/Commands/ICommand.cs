using MediatR;

namespace BuildingBlocks.Core.CQRS.Commands;

public interface ICommand : ICommand<Unit>
{ }

public interface ICommand<out T> : IRequest<T>
    where T : notnull
{ }