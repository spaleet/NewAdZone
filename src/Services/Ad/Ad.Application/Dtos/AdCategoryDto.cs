using System.Text.Json.Serialization;

namespace Ad.Application.Dtos;

public class AdCategoryDto
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Slug { get; set; }

    [JsonIgnore]
    public long? ParentId { get; set; }
}