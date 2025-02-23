using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notification.Domain.Notifications;
using Notification.Services.Interfaces;
using Notification.Services.Options;

namespace Notification.Services.AmazonSns
{
    public class AmazonSnsEmailMockService : IEmailProvider
    {
        private readonly AmazonSnsEmailServiceOptions _options;
        private readonly ILogger<AmazonSnsEmailMockService> _logger;

        public AmazonSnsEmailMockService(IOptions<AmazonSnsEmailServiceOptions> options, ILogger<AmazonSnsEmailMockService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public int GetPriority() => _options.Priority;

        public async Task SendEmailAsync(NotificationEmail notification)
        {
            //Implementation of AmazonSnsClient for sending Email

            //Random exception to simulate failures
            if (new Random().Next(2) == 0)
            {
                throw new Exception("This is mocked service... Error while connecting to server");
            }
            _logger.LogInformation("This is mocked service... AmazonSns Email send");
        }
    }
}
