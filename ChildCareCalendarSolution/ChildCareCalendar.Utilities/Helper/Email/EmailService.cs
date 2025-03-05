using ChildCareCalendar.Domain.ViewModels;
using ChildCareCalendar.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using MailKit.Security;
using MimeKit;
using MailKit.Net.Smtp;

namespace ChildCareCalendar.Utilities.Helper
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;


        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            email.To.Add(new MailboxAddress(toEmail, toEmail));
            email.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = body };
            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
