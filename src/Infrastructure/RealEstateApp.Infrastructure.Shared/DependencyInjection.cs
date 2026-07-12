using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Infrastructure.Shared.Settings;
// using RealEstateApp.Application.Interfaces.Shared; // Descomentar cuando existan las implementaciones
// using RealEstateApp.Infrastructure.Shared.Services; 

namespace RealEstateApp.Infrastructure.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<FileStorageSettings>(configuration.GetSection("FileStorageSettings"));

        // Cuando se creen las implementaciones de los servicios, se registran aquí:
        // services.AddTransient<IEmailService, EmailService>();
        // services.AddTransient<IFileStorageService, FileStorageService>();

        return services;
    }
}
