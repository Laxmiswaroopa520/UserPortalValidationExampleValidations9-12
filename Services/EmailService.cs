// This service is used to send real Emails.
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using UserPortalValdiationsDBContext.Interfaces;


namespace UserPortalValdiationsDBContext.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration Config;

        public EmailService(IConfiguration config)
        {
            Config = config;
        }

        public void SendEmail(string? to, string subject, string body)
        {
            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                Config["EmailSettings:SenderName"],
                Config["EmailSettings:SenderEmail"]
            ));

            email.To.Add(new MailboxAddress("", to));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            // Synchronous Connect
            smtp.Connect(
                Config["EmailSettings:SMTPHost"],
               int.Parse(Config["EmailSettings:SMTPPort"]),
            // int smtpPort = Config.GetValue<int>("EmailSettings:SMTPPort");
            SecureSocketOptions.StartTls
            );

            // Synchronous Authenticate
            smtp.Authenticate(
                Config["EmailSettings:SMTPUser"],
                Config["EmailSettings:SMTPPass"]
            );

            // Synchronous Send
            smtp.Send(email);

            // Synchronous Disconnect
            smtp.Disconnect(true);
        }
    }
}
