using Microsoft.Extensions.Options;
using RealEstateApp.Infrastructure.Shared.Services;
using RealEstateApp.Infrastructure.Shared.Settings;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Xunit;

namespace RealEstateApp.Infrastructure.Tests;

public class EmailServiceManualTest
{
    [Fact]
    public async Task SendAsync_ShouldSendEmail()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<EmailServiceManualTest>().Build();

  
        var settings = new EmailSettings
        {
            SenderName = "RealEstateApp Test",
            SenderEmail = "delismanuel13@gmail.com",
            SmtpHost = "smtp.gmail.com",
            SmtpPort = 587,
            UseSsl = true,
            Username = "delismanuel13@gmail.com", 
            Password = config["EmailSettings:Password"]!
        };
        
        var optionsMock = Options.Create(settings);
        var emailService = new EmailService(optionsMock);
        

        var destinationEmail = "Jesusortiz221516@gmail.com"; 
        await emailService.SendAsync(destinationEmail, "Prueba Manual RealEstateApp", "<h1>Test Exitoso</h1><p>El envío de correo funciona correctamente.</p>");
        
        Assert.True(true);
    }

    [Fact]
    public async Task SendAccountActivationEmailAsync_ShouldSendEmail()
    {
        var config = new ConfigurationBuilder().AddUserSecrets<EmailServiceManualTest>().Build();


        var settings = new EmailSettings
        {
            SenderName = "RealEstateApp Test",
            SenderEmail = "delismanuel13@gmail.com",
            SmtpHost = "smtp.gmail.com",
            SmtpPort = 587,
            UseSsl = true,
            Username = "delismanuel13@gmail.com", 
            Password = config["EmailSettings:Password"]!
        };

        var optionsMock = Options.Create(settings);
        var emailService = new EmailService(optionsMock);
        

        var destinationEmail = "Jesusortiz221516@gmail.com"; 
        await emailService.SendAccountActivationEmailAsync(destinationEmail, "Usuario de Prueba", "https://localhost:5001/api/account/activate?token=abc");
        

        Assert.True(true);
    }
}
