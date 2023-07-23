namespace Ad.Application.Dtos;

public class AdCategoryDto
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    public long? ParentId { get; set; }
}