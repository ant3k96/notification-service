namespace Notification.Domain.Notifications
{
    public class NotificationSms
    {
        public string Sender { get; init; } = default!;
        public string Receiver { get; init; } = default!;
        public string MessageBody { get; init; } = default!;
    }
}
