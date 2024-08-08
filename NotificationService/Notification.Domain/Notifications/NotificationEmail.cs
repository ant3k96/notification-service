namespace Notification.Domain.Notifications
{
    public class NotificationEmail
    {
        public string Sender { get; init; } = default!;
        public string Receiver { get; init; } = default!;
        public string Subject { get; init; } = default!;
        public string MessageBody { get; init; } = default!;
    }
}
