// for sending real emails
    namespace UserPortalValdiationsDBContext.Interfaces
    {
        public interface IEmailService
        {
            void SendEmail(string? to, string subject, string body);
        }
    }


