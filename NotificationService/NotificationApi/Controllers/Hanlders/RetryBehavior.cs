using MediatR;
using Notification.Api.Exceptions;
using Polly;
using Polly.Retry;

namespace Notification.Api.Controllers.Hanlders
{
    public class RetryDecorator<TNotification> : INotificationHandler<TNotification>
        where TNotification : INotification
    {
        private readonly INotificationHandler<TNotification> _inner;
        private readonly AsyncRetryPolicy _retryPolicy;

        public RetryDecorator(INotificationHandler<TNotification> inner)
        {
            // Define a retry policy with Polly
            _retryPolicy = Policy
                .Handle<ServiceUnavailableException>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(retryAttempt));
            _inner = inner;
        }
        public async Task Handle(TNotification request, CancellationToken cancellationToken)
        {
            await _retryPolicy.ExecuteAsync(() => _inner.Handle(request, cancellationToken));
        }
    }
}
