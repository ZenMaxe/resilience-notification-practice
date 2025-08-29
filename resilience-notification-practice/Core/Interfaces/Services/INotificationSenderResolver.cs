using resilience_notification_practice.Core.Models.Enums;

namespace resilience_notification_practice.Core.Interfaces.Services;

public interface INotificationSenderResolver
{
    ICollection<ISender> GetSenders();
    ISender GetSendersByNotificationType(NotificationType notificationType);
    ICollection<ISender> GetSendersByNotificationTypes(ICollection<NotificationType>  notificationTypes);
}