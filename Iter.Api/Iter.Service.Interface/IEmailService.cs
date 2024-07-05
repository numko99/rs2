using Iter.Core.Models;

namespace Iter.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage emailMessage);
    }
}
