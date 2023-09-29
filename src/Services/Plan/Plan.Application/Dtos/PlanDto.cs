namespace Plan.Application.Dtos;

public class PlanDto
{
    public string Id { get; set; }

    public string Title { get; set; }

    public int MonthlyQuota { get; set; }

    public decimal? Price { get; set; }
}