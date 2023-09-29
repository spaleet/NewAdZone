using Plan.Application.Dtos;

namespace Plan.Application.Features.GettingPlans;

public record GetPlansResponse(List<PlanDto>? Plans);