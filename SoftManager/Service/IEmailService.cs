namespace SoftManager.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string message);
    }
}
