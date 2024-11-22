using Microsoft.Extensions.Options;
using SoftManager.Models;
using System.Net.Mail;
using System.Net;

namespace SoftManager.Service
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value; // Obtém as configurações do arquivo appsettings.json
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Configuração do servidor SMTP
            var smtpClient = new SmtpClient(_emailSettings.SmtpServer)
            {
                Port = _emailSettings.SmtpPort,
                Credentials = new NetworkCredential(_emailSettings.SmtpUser, _emailSettings.SmtpPassword),
                EnableSsl = true,
            };

            // Criação do e-mail
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.FromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true, // Pode ser 'true' se o conteúdo do corpo for em HTML
            };

            mailMessage.To.Add(toEmail); // Adiciona o e-mail do destinatário

            // Envia o e-mail de forma assíncrona
            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
