// for sending real emails
namespace UserPortalValdiationsDBContext.Services.Interfaces
    {
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}


