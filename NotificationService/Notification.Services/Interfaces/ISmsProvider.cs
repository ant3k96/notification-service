using Notification.Domain.Notifications;

namespace Notification.Services.Interfaces
{
    public interface ISmsProvider : IPriorityService
    {
        Task SendSmsAsync(NotificationSms notification);
    }
}