using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Domain.Notifications;
using Notification.Services.Options;

namespace Notification.Services.Twilio
{
    public class TwilioMessageService : MessageProvider
    {
        private readonly TwilioMessageServiceOptions _options;
        private readonly ILogger<TwilioMessageService> _logger;

        public TwilioMessageService(IOptions<TwilioMessageServiceOptions> options, ILogger<TwilioMessageService> logger)
        {
            _options = options.Value;
            base.Priority = _options.Priority;
            _logger = logger;
        }


        public override async Task SendEmailAsync(NotificationMessage notification)
        {
            if (new Random().Next(2) == 0)
            {
                throw new Exception("Error while connecting to server");
            }
            _logger.LogInformation("Twilio Email Send");

        }

        public override async Task SendPushAsync(NotificationMessage notification)
        {
            _logger.LogInformation("Twilio Sms Send");
        }

        public override async Task SendSmsAsync(NotificationMessage notification)
        {
            _logger.LogInformation("Twilio Sms Send");
        }
    }
}