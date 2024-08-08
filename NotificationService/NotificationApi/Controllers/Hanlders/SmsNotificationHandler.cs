using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notification.Api.Exceptions;
using Notification.Api.Model;
using Notification.Api.Options;
using Notification.Domain.Notifications;
using Notification.Services;

namespace Notification.Api.Controllers.Hanlders
{
    public class SmsNotificationHandler : INotificationHandler<SendMessageRequest>
    {
        private readonly IEnumerable<IMessageProvider> _messagePublishers;
        private readonly IMapper _mapper;
        private readonly SmsChannelOptions _channelOptions;
        private readonly ILogger<SmsNotificationHandler> _logger;
        public SmsNotificationHandler(IEnumerable<IMessageProvider> messagePublishers, IMapper mapper, IOptions<SmsChannelOptions> options, ILogger<SmsNotificationHandler> logger)
        {
            _messagePublishers = messagePublishers;
            _mapper = mapper;
            _channelOptions = options.Value;
            _logger = logger;
        }
        public async Task Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            if (_channelOptions.Enabled)
            {
                if (request.Phone.From.IsNullOrEmpty() || request.Phone.To.IsNullOrEmpty())
                {
                    throw new InvalidMessageDataException("Email address cannot be null");
                }

                var notification = _mapper.Map<NotificationSms>(request);

                Type type;

                _messagePublishers.OrderBy(x => x.GetPriority());
                int callMaxCount = _messagePublishers.Count();
                int callCount = 0;
                foreach (var messagePublisher in _messagePublishers)
                {
                    type = messagePublisher.GetType();
                    try
                    {
                        await messagePublisher.SendSmsAsync(notification);
                    }
                    catch (Exception ex)
                    {
                        callCount++;
                        _logger.LogError(ex, "Error while sending Sms, to {ServiceName}", type.Name);
                        continue;
                    }
                    if (callCount == callMaxCount)
                    {
                        throw new ServiceUnavailableException("No service available at the moment");
                    }
                    break;
                }
            }
            else
            {
                _logger.LogDebug("Sms Notification channel disabled");
            }

        }
    }
}
