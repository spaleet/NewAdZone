using Ad.Application.Dtos;

namespace Ad.Application.Features.AdCategory.GettingAdCategories;

public record GetAdCategoriesResponse(List<AdCategoryOrderedResponse>? Categories);

public record AdCategoryOrderedResponse(AdCategoryDto Parent, List<AdCategoryDto>? Children);