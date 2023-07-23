using Ad.Application.Dtos;

namespace Ad.Application.Features.AdCategory.GettingAdCategories;

public record GetAdCategoriesResponse(List<AdCategoryOrderedResponse>? Categories);

public record AdCategoryOrderedResponse
{
	public AdCategoryOrderedResponse(AdCategoryDto parent, List<AdCategoryDto>? children)
	{
		Title = parent.Title;
        Slug = parent.Slug;
		Children = children;
	}

	public string Title { get; set; }

    public string Slug { get; set; }

    public List<AdCategoryDto>? Children { get; set; }
}