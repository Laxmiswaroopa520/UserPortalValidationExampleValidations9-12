using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;
using UserPortalValdiationsDBContext.Models.Config;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class TwilioSmsService : ISmsService
    {
        private readonly TwilioSettings _settings;

        public TwilioSmsService(IOptions<TwilioSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);

            await MessageResource.CreateAsync(
                body: message,
                from: new PhoneNumber(_settings.FromNumber),
                to: new PhoneNumber(phoneNumber)
            );
        }
    }
}
