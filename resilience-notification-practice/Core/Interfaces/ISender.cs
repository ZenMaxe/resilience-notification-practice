using resilience_notification_practice.Core.Models;
using resilience_notification_practice.Core.Models.Enums;

namespace resilience_notification_practice.Core.Interfaces;

public interface ISender
{
    NotificationType Type { get; }

    Task SendAsync(NotificationRequest notificationRequest, bool forceFail = false, CancellationToken cancellationToken = default);
}