using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Domain.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Context;
using RealEstateApp.Infrastructure.Persistence.Repositories;

namespace RealEstateApp.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistenceInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IPropertyRepository, PropertyRepository>();
        services.AddScoped<IPropertyAdminRepository, PropertyAdminRepository>();
        services.AddScoped<IOfferRepository, OfferRepository>();
        services.AddScoped<IMessageRepository, MessageRepository>();
        services.AddScoped<IFavoriteRepository, FavoriteRepository>();
        services.AddScoped<IPropertyTypeRepository, PropertyTypeRepository>();
        services.AddScoped<ISaleTypeRepository, SaleTypeRepository>();
        services.AddScoped<IImprovementRepository, ImprovementRepository>();
        services.AddScoped<IAgentQueryRepository, AgentQueryRepository>();

        return services;
    }
}