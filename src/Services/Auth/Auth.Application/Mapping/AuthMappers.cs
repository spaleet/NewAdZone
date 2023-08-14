using Auth.Application.Models;
using AutoMapper;

namespace Auth.Application.Mapping;

public class AuthMappers : Profile
{
    public AuthMappers()
    {
        CreateMap<RegisterAccountRequest, User>();
    }
}
