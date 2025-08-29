using resilience_notification_practice.Core.Models.Enums;

namespace resilience_notification_practice.DTOs.Requests;

public sealed record CreateNotificationRequestDto
{
    public string Receiver { get; init; }
    public string Message { get; init; }

    public ICollection<NotificationType> Methods { get; init; } = [];
    public ICollection<NotificationType> Fallbacks { get; init; } = [];
}