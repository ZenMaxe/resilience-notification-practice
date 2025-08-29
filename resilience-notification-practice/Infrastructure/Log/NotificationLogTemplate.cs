using resilience_notification_practice.Core.Models.Enums;

namespace resilience_notification_practice.Infrastructure.Log;

public static partial class NotificationLogTemplate
{
    [LoggerMessage(
        EventId = 1000,
        Level = LogLevel.Information,
        Message = "[Provider] {Type} sent: {Message}")]
    public static partial void ProviderSuccess(
        this ILogger logger,
        NotificationType type,
        string message);

    [LoggerMessage(
        EventId = 1001,
        Level = LogLevel.Error,
        Message = "[Provider] {Type} provider failed!")]
    public static partial void ProviderFailure(
        this ILogger logger,
        NotificationType type);
}