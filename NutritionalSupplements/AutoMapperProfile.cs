using AutoMapper;
using NutritionalSupplements.Data;
using NutritionalSupplements.Dto;

namespace NutritionalSupplements;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Provider, ProviderDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.RegistrationCountry, opt => opt.MapFrom(src => src.RegistrationCountry));

        CreateMap<Product, ProductDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate))
            .ForMember(dest => dest.ManufacturingDate, opt => opt.MapFrom(src => src.ManufacturingDate))
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider.Name));

        CreateMap<Ingredient, IngredientDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.IngredientSource, opt => opt.MapFrom(src => src.IngredientSource));

        CreateMap<NutritionalSupplement, NutritionalSupplementDTO>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.ENumber, opt => opt.MapFrom(src => src.ENumber))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.AcceptableDailyIntake, opt => opt.MapFrom(src => src.AcceptableDailyIntake));
        
        CreateMap<HealthEffect, HealthEffectDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

        CreateMap<Purpose, PurposeDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));

    }
}