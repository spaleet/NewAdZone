using Ad.Application.Dtos;

namespace Ad.Application.Features.Ad.GettingAd;
public record GetAdResponse : AdDto
{
    public AdCategoryDto Category { get; set; }
}
