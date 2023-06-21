using MediatR;

namespace BuildingBlocks.Core.CQRS.Queries;

public interface IQuery<out T> : IRequest<T>
    where T : notnull
{ }