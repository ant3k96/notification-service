using Notification.Domain.Notifications;

namespace Notification.Services.Interfaces
{
    public interface IEmailProvider : IPriorityService
    {
        Task SendEmailAsync(NotificationEmail notification);
    }
}