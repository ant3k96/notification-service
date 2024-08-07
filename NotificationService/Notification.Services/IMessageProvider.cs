using Notification.Domain.Notifications;

namespace Notification.Services
{
    public interface IMessageProvider
    {
        int GetPriority();
        Task SendSmsAsync(NotificationMessage notification);
        Task SendEmailAsync(NotificationMessage notification);
        Task SendPushAsync(NotificationMessage notification);
    }
}
