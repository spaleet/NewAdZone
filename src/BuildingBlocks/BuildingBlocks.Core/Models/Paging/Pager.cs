namespace BuildingBlocks.Core.Models.Paging;

public static class Pager
{
    public static PagingModel BuildPager(this PagingModel page, int allPagesCount)
    {
        int pageCount = Convert.ToInt32(Math.Ceiling(allPagesCount / (double)page.TakePage));
        return new PagingModel
        {
            PageId = page.PageId,
            DataCount = allPagesCount,
            TakePage = page.TakePage,
            SkipPage = (page.PageId - 1) * page.TakePage,
            StartPage = page.PageId - page.ShownPages <= 0 ? 1 : page.PageId - page.ShownPages,
            EndPage = page.PageId + page.ShownPages > pageCount ? pageCount : page.PageId + page.ShownPages,
            ShownPages = page.ShownPages,
            PageCount = pageCount
        };
    }
}