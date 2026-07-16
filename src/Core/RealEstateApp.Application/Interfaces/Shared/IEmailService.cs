namespace RealEstateApp.Application.Interfaces.Shared;

public interface IEmailService
{
    Task SendAsync(string toEmail, string subject, string htmlBody);

    // Método específico para el flujo de activación de cuenta del Cliente
    Task SendAccountActivationEmailAsync(string toEmail, string clientFirstName, string activationLink);
}