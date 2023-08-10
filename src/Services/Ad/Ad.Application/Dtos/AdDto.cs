using Ad.Domain.Enums;

namespace Ad.Application.Dtos;
public record AdDto
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public long CategoryId { get; set; }

    public string Price { get; set; }

    public string Title { get; set; }

    public string MainImage { get; set; }

    public SaleStatus SaleState { get; set; }

    public ProductStatus ProductState { get; set; }

    public string Description { get; set; }

    public string Tags { get; set; }
}
