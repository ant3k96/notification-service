using Notification.Domain.Notifications;

namespace Notification.Services
{
    public abstract class MessageProvider : IMessageProvider
    {
        protected int Priority { get; set; }

        public virtual int GetPriority()
        {
            return Priority;
        }
        public abstract Task SendSmsAsync(NotificationMessage notification);
        public abstract Task SendEmailAsync(NotificationMessage notification);
        public abstract Task SendPushAsync(NotificationMessage notification);
    }
}
