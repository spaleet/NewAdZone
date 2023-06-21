using AutoMapper;
using Plan.Application.Dtos;

namespace Plan.Application.Mapping;

public class PlanMapper : Profile
{
    public PlanMapper()
    {
        CreateMap<PlanDto, Domain.Entities.Plan>();
    }
}
