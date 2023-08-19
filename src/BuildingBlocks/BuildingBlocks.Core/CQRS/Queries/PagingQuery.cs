namespace BuildingBlocks.Core.CQRS.Queries;

public record PagingQuery<T> : IQuery<T> where T : notnull
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
