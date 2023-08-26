using Auth.Application.Models;
using AutoMapper;
using BuildingBlocks.Core.Utilities;

namespace Auth.Application.Mapping;

public class AuthMappers : Profile
{
    public AuthMappers()
    {
        CreateMap<RegisterAccountRequest, User>();

        CreateMap<User, UserProfileDto>()
            .ForMember(dest => dest.Id,
            opt => opt.MapFrom(src => src.Id.ToString()))
            .ForMember(dest => dest.RegisterDate,
            opt => opt.MapFrom(src => src.CreateDate.ToShamsi()));
    }
}
