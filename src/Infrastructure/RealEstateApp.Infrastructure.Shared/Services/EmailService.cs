using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using RealEstateApp.Application.Interfaces.Shared;
using RealEstateApp.Infrastructure.Shared.Settings;

namespace RealEstateApp.Infrastructure.Shared.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _emailSettings;

    public EmailService(IOptions<EmailSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.DisplayName, _emailSettings.EmailFrom));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = body };
        message.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();
        
        // APAGAR COMPLETAMENTE LA SEGURIDAD SSL LOCAL (Solo para desarrollo)
        smtp.CheckCertificateRevocation = false;
        smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
        
        // Usar SSL Directo forzado en lugar de Auto
        await smtp.ConnectAsync(_emailSettings.SmtpHost, _emailSettings.SmtpPort, SecureSocketOptions.SslOnConnect);
        await smtp.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass);
        
        await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);
    }

    public async Task SendAccountActivationEmailAsync(string email, string activationLink)
    {
        string subject = "Confirma tu cuenta - RealEstateApp";
        string body = $@"
            <h2>¡Bienvenido a RealEstateApp!</h2>
            <p>Para activar tu cuenta, haz clic en el siguiente enlace:</p>
            <a href='{activationLink}'>Confirmar mi cuenta</a>
            <p>Si no creaste esta cuenta, puedes ignorar este correo.</p>";

        await SendEmailAsync(email, subject, body);
    }
}
