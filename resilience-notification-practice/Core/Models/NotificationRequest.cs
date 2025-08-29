using resilience_notification_practice.Core.Models.Enums;

namespace resilience_notification_practice.Core.Models;

public class NotificationRequest
{
    public string Receiver { get; set; }
    public string Message { get; set; }

    public ICollection<NotificationType> Methods { get; set; } = [];
    public ICollection<NotificationType> Fallbacks { get; set; } = [];
}