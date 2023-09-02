using Ad.Domain.Entities;

namespace Ad.Application.Extensions;
public static class AdCategoryExtension
{
    public static async Task<string> JoinTags(this DbSet<AdCategory> query, long categoryId)
    {
        // map Tags based on categories
        // get category and parent category titles to an string array splitted by space
        string[] categories = await query.Include(x => x.ParentCategory)
            .Where(x => x.Id == categoryId)
            .Select(x => $"{x.Title} {x.ParentCategory.Title ?? string.Empty}".Trim().Split())
            .FirstOrDefaultAsync();

        // seperate titles by comma
        return string.Join(",", categories);
    }
}
