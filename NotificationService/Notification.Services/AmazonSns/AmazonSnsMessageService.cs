using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Domain.Notifications;
using Notification.Services.Options;

namespace Notification.Services.AmazonSns
{
    public partial class AmazonSnsMessageService : MessageProvider
    {
        private readonly AmazonSnsMessageServiceOptions _options;
        private readonly ILogger<AmazonSnsMessageService> _logger;

        public AmazonSnsMessageService(IOptions<AmazonSnsMessageServiceOptions> options, ILogger<AmazonSnsMessageService> logger)
        {
            _options = options.Value;
            base.Priority = _options.Priority;
            _logger = logger;
        }
        public override async Task SendEmailAsync(NotificationEmail notification)
        {
            if (new Random().Next(2) == 0)
            {
                throw new Exception("Error while connecting to server");
            }
            _logger.LogInformation("AmazonSns Email send");
        }

        public override async Task SendSmsAsync(NotificationSms notification)
        {
            _logger.LogInformation("AmazonSns Sms send");
        }
    }
}
