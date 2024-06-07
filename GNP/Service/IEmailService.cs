namespace GNP.Service
{
    public interface IEmailService
    {
        Task SendMailAsync(string to, string subject, string body);
    }
}
