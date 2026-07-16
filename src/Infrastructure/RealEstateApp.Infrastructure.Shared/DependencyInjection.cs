using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Infrastructure.Shared.Settings;
using RealEstateApp.Application.Interfaces.Shared;
using RealEstateApp.Infrastructure.Shared.Services; 

namespace RealEstateApp.Infrastructure.Shared;

public static class DependencyInjection
{
    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<FileStorageSettings>(configuration.GetSection("FileStorageSettings"));

        // Registrar servicios compartidos
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<IFileStorageService, FileStorageService>();

        return services;
    }
}
