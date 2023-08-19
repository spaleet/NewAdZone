using Ad.Application.Dtos;

namespace Ad.Application.Features.Ad.GettingAd;
public record GetAdDetailsResponse : AdDto
{
    public AdCategoryDto Category { get; set; }
}
