using resilience_notification_practice.Core.Interfaces;
using resilience_notification_practice.Core.Interfaces.Handlers;
using resilience_notification_practice.Core.Interfaces.Services;
using resilience_notification_practice.Core.Models;

namespace resilience_notification_practice.Core.Dispatchers;

public sealed class NotificationDispatcher : INotificationDispatcher
{
    private static Random _random = new();

    private readonly ILogger<NotificationDispatcher> _logger;
    private readonly INotificationSenderResolver _notificationSenderResolver;


    public NotificationDispatcher(ILogger<NotificationDispatcher> logger,
        INotificationSenderResolver notificationSenderResolver)
    {
        _logger = logger;
        _notificationSenderResolver = notificationSenderResolver;
    }

    public async Task SendNotificationAsync(NotificationRequest notificationRequest,
        CancellationToken cancellationToken = default)

    {
        if (notificationRequest.Methods.Count is 0)
        {
            _logger.LogWarning("No primary notification methods specified.");
            return;
        }

        var methods = _notificationSenderResolver
            .GetSendersByNotificationTypes(notificationRequest.Methods);

        bool success = await TrySendNotificationAsync(notificationRequest, methods, cancellationToken);

        if (success)
            return;


        if (notificationRequest.Fallbacks.Count is 0)
        {
            _logger.LogWarning("Primary methods failed and no fallbacks specified.");
            return;
        }


        var fallBackMethods = _notificationSenderResolver
            .GetSendersByNotificationTypes(notificationRequest.Fallbacks);


        success = await TrySendNotificationAsync(notificationRequest, fallBackMethods, cancellationToken);

        if (!success)
        {
            _logger.LogError("All fallback notification methods failed.");
        }
    }

    private async Task<bool> TrySendNotificationAsync(NotificationRequest notificationRequest,
        ICollection<ISender> senders,
        CancellationToken cancellationToken = default)
    {
        bool anySuccess = false;


        foreach (var sender in senders)
        {
            bool shouldFail = _random.Next(0, 100) < 20;

            try
            {
                await sender.SendAsync(notificationRequest, shouldFail, cancellationToken);
                _logger.LogInformation("{Sender} succeeded.", sender.GetType().Name);
                anySuccess = true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "{Sender} failed.", sender.GetType().Name);
            }
        }

        return anySuccess;
    }
}