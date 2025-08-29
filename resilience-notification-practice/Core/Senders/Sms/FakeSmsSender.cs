using resilience_notification_practice.Core.Models;
using resilience_notification_practice.Core.Models.Enums;
using resilience_notification_practice.Infrastructure.Log;

namespace resilience_notification_practice.Core.Senders.Sms;

public sealed class FakeSmsSender : ISmsSender
{
    public NotificationType Type => NotificationType.Sms;

    private readonly ILogger<FakeSmsSender> _logger;

    public FakeSmsSender(ILogger<FakeSmsSender> logger)
    {
        _logger = logger;
    }

    public async Task SendAsync(NotificationRequest notificationRequest, bool forceFail = false, CancellationToken cancellationToken = default)
    {
        await Task.Delay(Random.Shared.Next(200, 800), cancellationToken);

        if (forceFail)
        {
            throw new Exception("Sms provider failed!");
        }


        _logger.ProviderSuccess(Type, notificationRequest.Message);

    }
}