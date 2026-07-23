using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.Services;
using RealEstateApp.Domain.Factories.Interfaces;
using RealEstateApp.Domain.Factories.Implementations;
using RealEstateApp.Domain.Services.Interfaces;
using RealEstateApp.Domain.Services.Implementations;
using System.Reflection;

namespace RealEstateApp.Application;

public static class ServiceRegistration
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(config => { }, Assembly.GetExecutingAssembly());
        
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IAgentService, AgentService>();
        services.AddTransient<IFavoriteService, FavoriteService>();
        services.AddTransient<IImprovementService, ImprovementService>();
        services.AddTransient<IMessageService, MessageService>();
        services.AddTransient<IOfferService, OfferService>();
        services.AddTransient<IPropertyService, PropertyService>();
        services.AddTransient<IPropertyTypeService, PropertyTypeService>();
        services.AddTransient<ISaleTypeService, SaleTypeService>();

        services.AddTransient<IFavoriteFactory, FavoriteFactory>();
        services.AddTransient<IImprovementFactory, ImprovementFactory>();
        services.AddTransient<IMessageFactory, MessageFactory>();
        services.AddTransient<IOfferFactory, OfferFactory>();
        services.AddTransient<IPropertyFactory, PropertyFactory>();
        services.AddTransient<IPropertyTypeFactory, PropertyTypeFactory>();
        services.AddTransient<ISaleTypeFactory, SaleTypeFactory>();

        services.AddTransient<IOfferDomainService, OfferDomainService>();
        services.AddTransient<IPropertyCodeDomainService, PropertyCodeDomainService>();
        services.AddTransient<IPropertyDomainService, PropertyDomainService>();
    }
}
