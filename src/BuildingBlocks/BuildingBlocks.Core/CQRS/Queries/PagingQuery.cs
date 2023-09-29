using System.Text.Json.Serialization;

namespace BuildingBlocks.Core.CQRS.Queries;

public record PagingQuery<T> : IQuery<T> where T : notnull
{
    [JsonPropertyName("pageSize")]
    public int Page { get; set; } = 1;

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; } = 10;
}