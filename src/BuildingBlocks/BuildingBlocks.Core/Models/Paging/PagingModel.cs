using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BuildingBlocks.Core.Models.Paging;

public class PagingModel<T>
{
    public PagingModel(List<T> items, long totalItems, int page, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        Page = page;
        PageSize = pageSize;
    }

    public int Page { get; set; }

    public int PageSize { get; set; }

    [BindNever]
    public List<T> Items { get; set; }

    [BindNever]
    public long TotalItems { get; set; }

    public PagingDataSortCreationDateOrder SortDateOrder { get; set; } = PagingDataSortCreationDateOrder.DES;

    public PagingDataSortIdOrder SortIdOrder { get; set; } = PagingDataSortIdOrder.NotSelected;


    //================================== Methods
    public static PagingModel<T> Empty => new(Enumerable.Empty<T>().ToList(), 0, 0, 0);
    
    public static PagingModel<T> Create(List<T> items, long totalItems = 0, int page = 1, int pageSize = 20)
    {
        return new PagingModel<T>(items, totalItems, page, pageSize);
    }
}

public enum PagingDataSortIdOrder
{
    NotSelected,
    DES,
    ASC
}

public enum PagingDataSortCreationDateOrder
{
    DES,
    ASC
}