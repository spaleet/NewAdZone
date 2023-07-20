using Ad.Application.Dtos;
using Ad.Domain.Entities;
using AutoMapper;

namespace Ad.Application.Mapping;

public class AdMapper : Profile
{
    public AdMapper()
    {
        CreateMap<AdCategory, AdCategoryDto>();
    }
}
