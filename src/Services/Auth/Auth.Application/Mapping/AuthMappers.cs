using Auth.Application.Models;
using Auth.Domain.Entities;
using AutoMapper;

namespace Auth.Application.Mapping;

public class AuthMappers : Profile
{
    public AuthMappers()
    {
        CreateMap<RegisterAccountRequest, User>();
    }
}
