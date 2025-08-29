using resilience_notification_practice.Core.Interfaces;
using resilience_notification_practice.Core.Interfaces.Services;
using resilience_notification_practice.Core.Models.Enums;

namespace resilience_notification_practice.Core.Services;

public sealed class NotificationSenderResolver : INotificationSenderResolver
{
    private static Dictionary<NotificationType, ISender> _senders = new();

    public NotificationSenderResolver(IEnumerable<ISender>  senders)
    {
        SetCategorizedSenders(senders);
    }


    public ICollection<ISender> GetSenders()
    {
        return _senders
            .Select(x => x.Value)
            .ToList();
    }


    public ISender GetSendersByNotificationType(NotificationType notificationType)
    {
        if (_senders.TryGetValue(notificationType, out var sender))
        {
            return sender;
        }

        throw new ArgumentException("Invalid notification type");
    }

    public ICollection<ISender> GetSendersByNotificationTypes(ICollection<NotificationType> notificationTypes)
    {
        return _senders
            .Where(x => notificationTypes.Contains(x.Key))
            .Select(x => x.Value)
            .ToList();
    }




    private void SetCategorizedSenders(IEnumerable<ISender> senders)
    {
        _senders
            = senders
                .ToDictionary(x => x.Type,
                    x => x);
    }
}