﻿using Ad.Application.Dtos;
using Ad.Application.Features.Ad.EditingAd;
using Ad.Application.Features.Ad.GettingAd;
using Ad.Application.Features.Ad.PostingAd;
using Ad.Application.Features.AdCategory.CreatingAdCategory;
using Ad.Domain.Entities;
using AutoMapper;
using BuildingBlocks.Core.Utilities;

namespace Ad.Application.Mapping;

public class AdMapper : Profile
{
    public AdMapper()
    {
        CreateMap<AdCategory, AdCategoryDto>();

        CreateMap<CreateAdCategory, AdCategory>()
            .ForMember(dest => dest.Slug,
                           opt => opt.MapFrom(src => src.Title.ToSlug()));

        CreateMap<Domain.Entities.Ad, AdDto>()
            .ForMember(dest => dest.Description,
                           opt => opt.MapFrom(src => src.Description.Substring(0, 20) + " ..."))
            .ForMember(dest => dest.Price,
                           opt => opt.MapFrom(src => src.Price == 0 ? "-" : src.Price.ToMoney()));

        CreateMap<Domain.Entities.Ad, GetAdDetailsResponse>()
            .ForMember(dest => dest.Category,
                           opt => opt.MapFrom(src => src.AdCategory))
            .ForMember(dest => dest.Price,
                           opt => opt.MapFrom(src => src.Price == 0 ? "-" : src.Price.ToMoney()));

        CreateMap<PostAd, Domain.Entities.Ad>()
            .ForMember(dest => dest.CategoryId,
                           opt => opt.MapFrom(src => src.SelectedCategory))
            .ForMember(dest => dest.Slug,
                           opt => opt.MapFrom(src => src.Title.ToSlug()));

        CreateMap<EditAd, Domain.Entities.Ad>()
            .ForMember(dest => dest.Id,
                           opt => opt.Ignore())
            .ForMember(dest => dest.CategoryId,
                           opt => opt.MapFrom(src => src.SelectedCategory))
            .ForMember(dest => dest.Slug,
                           opt => opt.MapFrom(src => src.Title.ToSlug()));
    }
}