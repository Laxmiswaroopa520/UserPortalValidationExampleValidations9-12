using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(to))
                throw new ArgumentException("Recipient email cannot be empty", nameof(to));

            var email = new MimeMessage();

            email.From.Add(new MailboxAddress(
                _config["EmailSettings:SenderName"],
                _config["EmailSettings:SenderEmail"]
            ));

            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = subject;

            email.Body = new BodyBuilder
            {
                HtmlBody = body
            }.ToMessageBody();

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _config["EmailSettings:SMTPHost"],
                _config.GetValue<int>("EmailSettings:SMTPPort"),
                SecureSocketOptions.StartTls
            );

            await smtp.AuthenticateAsync(
                _config["EmailSettings:SMTPUser"],
                _config["EmailSettings:SMTPPass"]
            );

            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}


/*| Issue                                | Why improve                  |
| ------------------------------------ | ---------------------------- |
| Synchronous SMTP calls               | Blocks threads               |
| Hardcoded config access inside logic | Harder to test               |
| No null guard on to                | Possible runtime error       |
| No async support                     | Modern ASP.NET prefers async |
| No exception context                 | Harder debugging             |
*/
/*These are the problems in my previous Email Service..  */
















/* This service is used to send real Emails.
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using UserPortalValdiationsDBContext.Services.Interfaces;


namespace UserPortalValdiationsDBContext.Services.Implementations
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
*/