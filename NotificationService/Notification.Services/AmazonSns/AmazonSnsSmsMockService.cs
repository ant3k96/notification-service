using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Domain.Notifications;
using Notification.Services.Interfaces;
using Notification.Services.Options;

namespace Notification.Services.AmazonSns
{
    public class AmazonSnsSmsMockService : ISmsProvider
    {
        private readonly AmazonSnsSmsServiceOptions _options;
        private readonly ILogger<AmazonSnsSmsMockService> _logger;

        public AmazonSnsSmsMockService(IOptions<AmazonSnsSmsServiceOptions> options, ILogger<AmazonSnsSmsMockService> logger)
        {
            _logger = logger;
            _options = options.Value;
        }
        public int GetPriority()
        {
            return _options.Priority;
        }

        public async Task SendSmsAsync(NotificationSms notification)
        {
            _logger.LogInformation("This is mocked service... AmazonSns Sms send");
        }
    }
}
