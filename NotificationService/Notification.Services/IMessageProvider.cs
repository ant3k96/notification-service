using Notification.Domain.Notifications;

namespace Notification.Services
{
    public interface IMessageProvider
    {
        int GetPriority();
        Task SendSmsAsync(NotificationSms notification);
        Task SendEmailAsync(NotificationEmail notification);
    }
}
