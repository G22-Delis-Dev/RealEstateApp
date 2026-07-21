using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Application.Interfaces.Services;
using RealEstateApp.Application.Services;

namespace RealEstateApp.Application;

public static class ServiceRegistration
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        #region Services
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IPropertyService, PropertyService>();
        services.AddTransient<IPropertyTypeService, PropertyTypeService>();
        services.AddTransient<ISaleTypeService, SaleTypeService>();
        services.AddTransient<IImprovementService, ImprovementService>();
        services.AddTransient<IAgentService, AgentService>();
        services.AddTransient<IOfferService, OfferService>();
        services.AddTransient<IFavoriteService, FavoriteService>();
        services.AddTransient<IMessageService, MessageService>();
        #endregion
    }
}
