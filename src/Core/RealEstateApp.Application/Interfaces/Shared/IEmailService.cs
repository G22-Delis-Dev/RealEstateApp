using System.Threading.Tasks;

namespace RealEstateApp.Application.Interfaces.Shared;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    
    // NUEVO - rúbrica: método específico para el correo de activación de cuenta
    Task SendAccountActivationEmailAsync(string email, string activationLink);
}
