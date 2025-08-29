using resilience_notification_practice.Core.Models;

namespace resilience_notification_practice.Core.Interfaces.Handlers;

public interface INotificationDispatcher
{
    Task SendNotificationAsync(NotificationRequest notificationRequest,
        CancellationToken cancellationToken = default);
}