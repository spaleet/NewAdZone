using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Core.Models.Paging;

public static class PagingExtension
{
    public static async Task<PagingModel<T>> ApplyPagingAsync<T>(
        this IQueryable<T> collection,
        int page = 1,
        int pageSize = 10,
        CancellationToken cancellationToken = default
    )
        where T : notnull
    {
        if (page <= 0)
            page = 1;

        if (pageSize <= 0)
            pageSize = 10;

        var isEmpty = await collection.AnyAsync(cancellationToken: cancellationToken) == false;
        if (isEmpty)
            return PagingModel<T>.Empty;

        var totalItems = await collection.CountAsync(cancellationToken: cancellationToken);
        var totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
        var data = await collection.Limit(page, pageSize).ToListAsync(cancellationToken: cancellationToken);

        return PagingModel<T>.Create(data, totalItems, page, pageSize);
    }
}
