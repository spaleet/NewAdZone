using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Plan.Application.Features.CreatingNewPlan;

public class CreateNewPlanRequest
{
    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("adQuota")]
    public int AdQuota { get; set; }

    [Display(Name = "قیمت پلن")]
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }
}
