using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using RealEstateApp.Application.Interfaces.Shared;
using RealEstateApp.Infrastructure.Shared.Settings;

namespace RealEstateApp.Infrastructure.Shared.Services;

public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task SendAsync(string toEmail, string subject, string htmlBody)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        message.Body = new BodyBuilder
        {
            HtmlBody = htmlBody
        }.ToMessageBody();

        using var client = new SmtpClient();

        // APAGAR COMPLETAMENTE LA SEGURIDAD SSL LOCAL (Solo para desarrollo)
        client.CheckCertificateRevocation = false;
        client.ServerCertificateValidationCallback = (s, c, h, e) => true;

        await client.ConnectAsync(
            _settings.SmtpHost,
            _settings.SmtpPort,
            SecureSocketOptions.SslOnConnect);

        await client.AuthenticateAsync(_settings.Username, _settings.Password);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendAccountActivationEmailAsync(string toEmail, string clientFirstName, string activationLink)
    {
        const string subject = "Activación de cuenta en RealEstateApp";

        var htmlBody = $@"
            <div style=""font-family: Arial, sans-serif; max-width: 480px; margin: 0 auto;"">
                <h2 style=""color:#16233B;"">Hola {clientFirstName},</h2>
                <p>Su cuenta ha sido registrada correctamente en <strong>RealEstateApp</strong>.</p>
                <p>Para activar su usuario y poder iniciar sesión, haga clic en el siguiente enlace:</p>
                <p style=""margin: 24px 0;"">
                    <a href=""{activationLink}""
                       style=""background:#B8935B; color:#fff; padding:12px 24px; text-decoration:none; font-weight:bold;"">
                       Activar mi cuenta
                    </a>
                </p>
                <p style=""color:#5C6773; font-size:12px;"">
                    Si usted no realizó este registro, puede ignorar este mensaje.
                </p>
            </div>";

        await SendAsync(toEmail, subject, htmlBody);
    }
}
