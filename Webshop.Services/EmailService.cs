using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace Webshop.Services
{
    public class EmailService
    {
        private readonly string _smtpEmail;
        private readonly string _smtpPassword;
        private readonly string _smtpServer;

        public EmailService(IConfiguration configuration)
        {
            _smtpEmail = configuration["SMTP:Email"]!;
            _smtpPassword = configuration["SMTP:Password"]!;
            _smtpServer = configuration["SMTP:Server"]!;
        }

        public async Task SendPasswordResetEmail(string email, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Webshop", _smtpEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Password Reset Request";
            message.Body = new TextPart("plain")
            {
                Text = $"Please reset your password by clicking the following link: {resetLink}"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpEmail, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task SendPasswordChangedNotification(string email)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Webshop", "no-reply@webshop.com"));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = "Password Changed";
            message.Body = new TextPart("plain")
            {
                Text = "Your password has been changed. If you did not request this change, please contact support immediately."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync(_smtpServer, 587, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_smtpEmail, _smtpPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
