namespace GNP.IRepository
{
    public interface IEmailService
    {
        Task SendMailAsync(string to, string subject, string body);
    }
}
