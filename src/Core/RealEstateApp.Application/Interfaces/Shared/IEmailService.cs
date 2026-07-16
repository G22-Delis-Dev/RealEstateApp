using System.Threading.Tasks;

namespace RealEstateApp.Application.Interfaces.Shared;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body);
    Task SendAccountActivationEmailAsync(string email, string activationLink);
}
