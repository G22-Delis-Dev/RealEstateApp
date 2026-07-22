using AutoMapper;
using RealEstateApp.Application.DTOs.Agents;
using RealEstateApp.Application.DTOs.Catalogs;
using RealEstateApp.Application.DTOs.Properties;
using RealEstateApp.Application.ViewModels.Catalogs;
using RealEstateApp.Application.ViewModels.Messages;
using RealEstateApp.Application.ViewModels.Offers;
using RealEstateApp.Application.ViewModels.Properties;
using RealEstateApp.Domain.Entities;

namespace RealEstateApp.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Property, PropertyViewModel>()
            .ForMember(d => d.PropertyTypeId, o => o.MapFrom(s => s.PropertyTypeId))
            .ForMember(d => d.PropertyTypeName, o => o.MapFrom(s => s.PropertyType.Name))
            .ForMember(d => d.SaleTypeId, o => o.MapFrom(s => s.SaleTypeId))
            .ForMember(d => d.SaleTypeName, o => o.MapFrom(s => s.SaleType.Name))
            .ForMember(d => d.ImageUrls, o => o.MapFrom(s => s.Images.Select(i => i.Url)))
            .ForMember(d => d.ImprovementIds, o => o.MapFrom(s => s.Improvements.Select(i => i.Id)))
            .ForMember(d => d.Improvements, o => o.MapFrom(s => s.Improvements.Select(i => i.Name)))
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<Offer, OfferViewModel>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<Message, MessageViewModel>();

        CreateMap<PropertyType, PropertyTypeViewModel>()
            .ForMember(d => d.PropertyCount, o => o.MapFrom(s => s.Properties.Count));

        CreateMap<SaleType, SaleTypeViewModel>()
            .ForMember(d => d.PropertyCount, o => o.MapFrom(s => s.Properties.Count));

        CreateMap<Improvement, ImprovementViewModel>()
            .ForMember(d => d.PropertyCount, o => o.MapFrom(s => s.Properties.Count));

        CreateMap<Property, PropertyDto>()
            .ForMember(d => d.PropertyTypeName, o => o.MapFrom(s => s.PropertyType.Name))
            .ForMember(d => d.SaleTypeName, o => o.MapFrom(s => s.SaleType.Name))
            .ForMember(d => d.Improvements, o => o.MapFrom(s => s.Improvements.Select(i => i.Name)))
            .ForMember(d => d.AgentId, o => o.MapFrom(s => s.AgentId))
            .ForMember(d => d.AgentName, o => o.Ignore()) // se completa en el Service, viene de Identity, no de esta Entity
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()));

        CreateMap<PropertyType, PropertyTypeDto>();
        CreateMap<SaleType, SaleTypeDto>();
        CreateMap<Improvement, ImprovementDto>();

        // AgentDto NO mapea desde una Entity de Domain (el agente vive en Identity) —
        // se construye manualmente en AgentService combinando ApplicationUser + conteo de propiedades.
    }
}