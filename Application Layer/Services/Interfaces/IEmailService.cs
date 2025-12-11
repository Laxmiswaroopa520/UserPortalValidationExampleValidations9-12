// for sending real emails
namespace UserPortalValdiationsDBContext.Services.Interfaces
    {
        public interface IEmailService
        {
            void SendEmail(string? to, string subject, string body);
        }
    }


