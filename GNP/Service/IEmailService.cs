using GeneralWorkPermit.EmailService;

namespace GNP.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(EmailMessage message);
    }
}
