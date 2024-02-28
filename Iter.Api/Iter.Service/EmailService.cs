using Iter.Core.Options;
using Iter.Services.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Iter.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string to, string subject, string content)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailSettings.UserName, _emailSettings.UserName));
            emailMessage.To.Add(MailboxAddress.Parse(to));
            emailMessage.Subject = subject;

            var path = "C:\\Projects\\rs2\\Iter.Api\\Iter.Core\\Static files\\emailTemplate.html";
            var htmlTemplate = await File.ReadAllTextAsync(path);

            htmlTemplate = htmlTemplate.Replace("{{Title}}", subject);
            htmlTemplate = htmlTemplate.Replace("{{Content}}", content);

            emailMessage.Body = new TextPart("html") { Text = htmlTemplate };

            using var client = new SmtpClient();
            await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
            await client.SendAsync(emailMessage);
            await client.DisconnectAsync(true);
        }
    }
}