using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Domain.Notifications;
using Notification.Services.Interfaces;
using Notification.Services.Options;

namespace Notification.Services.Twilio
{
    public class TwilioSendGridEmailMockService : IEmailProvider
    {
        private readonly TwilioSmsServiceOptions _options;
        private readonly ILogger<TwilioSendGridEmailMockService> _logger;

        public TwilioSendGridEmailMockService(IOptions<TwilioSmsServiceOptions> options, ILogger<TwilioSendGridEmailMockService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public int GetPriority() => _options.Priority;

        public async Task SendEmailAsync(NotificationEmail notification)
        {
            //Random exception to simulate failures

            if (new Random().Next(2) == 0)
            {
                throw new Exception("This is mocked service... Error while connecting to server");
            }
            _logger.LogInformation("This is mocked service... Twilio Email Send");
        }

    }
}