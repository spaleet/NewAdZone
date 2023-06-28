using AutoMapper;
using Plan.Application.Dtos;
using Plan.Application.Features.CreatingNewPlan;

namespace Plan.Application.Mapping;

public class PlanMapper : Profile
{
    public PlanMapper()
    {
        CreateMap<Domain.Entities.Plan, PlanDto>();
        CreateMap<CreateNewPlan, Domain.Entities.Plan>();
    }
}
