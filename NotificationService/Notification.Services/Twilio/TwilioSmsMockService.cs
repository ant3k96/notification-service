using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Domain.Notifications;
using Notification.Services.Interfaces;
using Notification.Services.Options;

namespace Notification.Services.Twilio
{
    public class TwilioSmsMockService : ISmsProvider
    {
        private readonly TwilioSmsServiceOptions _options;
        private readonly ILogger<TwilioSmsMockService> _logger;

        public TwilioSmsMockService(IOptions<TwilioSmsServiceOptions> options, ILogger<TwilioSmsMockService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public int GetPriority() => _options.Priority;

        public async Task SendSmsAsync(NotificationSms notification)
        {
            _logger.LogInformation("This is mocked service... Twilio Sms Send");
        }
    }
}
