using MapsterMapper;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Notification.Api.Exceptions;
using Notification.Api.Model;
using Notification.Api.Options;
using Notification.Domain.Notifications;
using Notification.Services.Interfaces;

namespace Notification.Api.Controllers.Handlers
{
    public class SmsNotificationHandler : INotificationHandler<SendMessageRequest>
    {
        private readonly IEnumerable<ISmsProvider> _smsProviders;
        private readonly IMapper _mapper;
        private readonly SmsChannelOptions _channelOptions;
        private readonly ILogger<SmsNotificationHandler> _logger;
        public SmsNotificationHandler(IEnumerable<ISmsProvider> smsProviders, IMapper mapper, IOptions<SmsChannelOptions> options, ILogger<SmsNotificationHandler> logger)
        {
            _smsProviders = smsProviders;
            _mapper = mapper;
            _channelOptions = options.Value;
            _logger = logger;
        }
        public async Task Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            if (_channelOptions.Enabled)
            {
                if (request.Sms.From.IsNullOrEmpty() || request.Sms.To.IsNullOrEmpty())
                {
                    throw new InvalidMessageDataException("Email address cannot be null");
                }

                var notification = _mapper.Map<NotificationSms>(request);

                Type type;

                int callMaxCount = _smsProviders.Count();
                int callCount = 0;
                foreach (var provider in _smsProviders.OrderBy(x => x.GetPriority()))
                {
                    type = provider.GetType();
                    try
                    {
                        await provider.SendSmsAsync(notification);
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
