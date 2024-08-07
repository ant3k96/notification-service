namespace Notification.Domain.Notifications
{
    public class NotificationMessage
    {
        public required string Sender { get; init; }
        public required string Receiver { get; init; }
        public required string MessageBody { get; init; }
    }
}
