using resilience_notification_practice.Core.Models;
using resilience_notification_practice.Core.Models.Enums;
using resilience_notification_practice.Infrastructure.Log;

namespace resilience_notification_practice.Core.Senders.Push;

public sealed class FakePushSender : IPushSender
{
    public NotificationType Type => NotificationType.Push;

    private readonly ILogger<FakePushSender> _logger;

    public FakePushSender(ILogger<FakePushSender> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(NotificationRequest notificationRequest, bool forceFail = false, CancellationToken cancellationToken = default)
    {
        await Task.Delay(Random.Shared.Next(200, 800), cancellationToken);

        if (forceFail)
        {
            throw new Exception("Push provider failed!");
        }


        _logger.ProviderSuccess(Type, notificationRequest.Message);
    }
}