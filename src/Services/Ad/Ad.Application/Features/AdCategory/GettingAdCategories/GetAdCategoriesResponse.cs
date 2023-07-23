using Ad.Application.Dtos;

namespace Ad.Application.Features.AdCategory.GettingAdCategories;

public record GetAdCategoriesResponse(List<AdCategoryDto>? Categories);
