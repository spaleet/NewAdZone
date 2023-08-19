namespace BuildingBlocks.Core.Models.Paging;

public class PagingModel<T>
{
    public PagingModel(List<T> items, int totalItems, int totalPages, int page, int pageSize)
    {
        Items = items;
        TotalItems = totalItems;
        TotalPages = totalPages;
        Page = page;
        PageSize = pageSize;
    }

    public int Page { get; set; }

    public int PageSize { get; set; }

    public List<T> Items { get; set; }

    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    //================================== Methods
    public static PagingModel<T> Empty => new(Enumerable.Empty<T>().ToList(), 0, 0, 0, 0);

    public static PagingModel<T> Create(List<T> items, int totalItems = 0, int totalPages = 0, int page = 1, int pageSize = 20)
    {
        return new PagingModel<T>(items, totalItems, totalPages, page, pageSize);
    }
}