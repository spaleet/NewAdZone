using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace BuildingBlocks.Core.Models.Paging;

public static class PagingExtension
{
    // Paging
    public static async Task<PagingModel<T>> ApplyPagingAsync<T>(this IQueryable<T> collection, 
        int page = 1, int pageSize = 10) where T : notnull
    {
        if (page <= 0)
            page = 1;

        if (pageSize <= 0)
            pageSize = 10;

        bool isEmpty = await collection.AnyAsync() == false;

        if (isEmpty)
            return PagingModel<T>.Empty;

        int totalItems = await collection.CountAsync();
        int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

        var data = await collection.Limit(page, pageSize).ToListAsync();

        return PagingModel<T>.Create(data, totalItems, totalPages, page, pageSize);
    }

    // Paging with Mapping
    public static async Task<PagingModel<TOut>> ApplyPagingAsync<TIn, TOut>(this IQueryable<TIn> collection,
       AutoMapper.IConfigurationProvider configuration, int page = 1, int pageSize = 10) where TOut : notnull
    {
        if (page <= 0)
            page = 1;

        if (pageSize <= 0)
            pageSize = 10;

        bool isEmpty = await collection.AnyAsync() == false;
        if (isEmpty)
            return PagingModel<TOut>.Empty;

        int totalItems = await collection.CountAsync();
        int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

        var data = await collection
            .Limit(page, pageSize)
            .ProjectTo<TOut>(configuration) // using automapper
            .ToListAsync();

        return PagingModel<TOut>.Create(data, totalItems, totalPages, page, pageSize);
    }

    // ------- Syncs Methods:

    // Paging
    public static PagingModel<T> ApplyPagingSync<T>(this IQueryable<T> collection, 
        int page = 1, int pageSize = 10) where T : notnull
    {
        if (page <= 0)
            page = 1;

        if (pageSize <= 0)
            pageSize = 10;

        bool isEmpty = collection.Any() == false;

        if (isEmpty)
            return PagingModel<T>.Empty;

        int totalItems = collection.Count();
        int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

        var data = collection.Limit(page, pageSize).ToList();

        return PagingModel<T>.Create(data, totalItems, totalPages, page, pageSize);
    }

    // Paging with Mapping
    public static PagingModel<TOut> ApplyPagingSync<TIn, TOut>(this IQueryable<TIn> collection,
       AutoMapper.IConfigurationProvider configuration, int page = 1, int pageSize = 10) where TOut : notnull
    {
        if (page <= 0)
            page = 1;

        if (pageSize <= 0)
            pageSize = 10;

        bool isEmpty = collection.Any() == false;
        if (isEmpty)
            return PagingModel<TOut>.Empty;

        int totalItems = collection.Count();
        int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);

        var data = collection
            .Limit(page, pageSize)
            .ProjectTo<TOut>(configuration) // using automapper
            .ToList();

        return PagingModel<TOut>.Create(data, totalItems, totalPages, page, pageSize);
    }

    // Simple paging
    public static IQueryable<TEntity> ApplyPaging<TEntity>(this IQueryable<TEntity> source, int page, int size)
       where TEntity : class
    {
        return source.Skip(page * size).Take(size);
    }
}

public static class EfPagingExtension
{
    public static IQueryable<T> Limit<T>(this IQueryable<T> collection, int page = 1, int resultsPerPage = 10)
    {
        if (page <= 0)
            page = 1;

        if (resultsPerPage <= 0)
            resultsPerPage = 10;

        int skip = (page - 1) * resultsPerPage;
        var data = collection.Skip(skip).Take(resultsPerPage);

        return data;
    }
}