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
    public class EmailNotificationHandler : INotificationHandler<SendMessageRequest>
    {
        private readonly IEnumerable<IEmailProvider> _emailProviders;
        private readonly IMapper _mapper;
        private readonly EmailChannelOptions _channelOptions;
        private readonly ILogger<EmailNotificationHandler> _logger;
        public EmailNotificationHandler(IEnumerable<IEmailProvider> emailProviders, IMapper mapper, IOptions<EmailChannelOptions> options
            , ILogger<EmailNotificationHandler> logger)
        {
            _emailProviders = emailProviders;
            _mapper = mapper;
            _channelOptions = options.Value;
            _logger = logger;
        }
        public async Task Handle(SendMessageRequest request, CancellationToken cancellationToken)
        {
            if (_channelOptions.Enabled)
            {
                if (request.Email.From.IsNullOrEmpty() || request.Email.To.IsNullOrEmpty())
                {
                    throw new InvalidMessageDataException("Email address cannot be null");
                }
                var notification = _mapper.Map<NotificationEmail>(request);

                Type type;

                int callMaxCount = _emailProviders.Count();
                int callCount = 0;
                foreach (var messagePublisher in _emailProviders.OrderBy(e => e.GetPriority()))
                {
                    type = messagePublisher.GetType();
                    try
                    {
                        await messagePublisher.SendEmailAsync(notification);
                    }
                    catch (Exception ex)
                    {
                        callCount++;
                        _logger.LogError(ex, "Error while sending email, to {ServiceName}", type.Name);
                        continue;
                    }
                    break;
                }
                if (callCount == callMaxCount)
                {
                    throw new ServiceUnavailableException("No service available at the moment");
                }
            }
            else
            {
                _logger.LogDebug("Email Notification channel disabled");
            }

        }
    }
}
