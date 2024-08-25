using Iter.Core.Models;
using Iter.Core.Options;
using Iter.Services.Interface;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Iter.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(EmailMessage emailData)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress(_emailSettings.UserName, _emailSettings.UserName));
                emailMessage.To.Add(MailboxAddress.Parse(emailData.Email));
                emailMessage.Subject = emailData.Subject;

                var assembly = typeof(EmailService).Assembly;
                var assemblyName = assembly.GetName().Name;
                var resourceName = $"{assemblyName}.emailTemplate.html";

                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream == null)
                {
                    throw new FileNotFoundException($"The resource {resourceName} was not found.");
                }

                using var reader = new StreamReader(stream);
                var htmlTemplate = await reader.ReadToEndAsync();

                htmlTemplate = htmlTemplate.Replace("{{Title}}", emailData.Subject);
                htmlTemplate = htmlTemplate.Replace("{{Content}}", emailData.Content);

                emailMessage.Body = new TextPart("html") { Text = htmlTemplate };

                using var client = new SmtpClient();
                await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.UserName, _emailSettings.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
