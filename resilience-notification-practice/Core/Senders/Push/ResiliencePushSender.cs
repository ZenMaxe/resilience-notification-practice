using Polly;
using resilience_notification_practice.Core.Models;
using resilience_notification_practice.Core.Models.Enums;
using resilience_notification_practice.Infrastructure.Log;

namespace resilience_notification_practice.Core.Senders.Push;

public sealed class ResiliencePushSender : IPushSender
{
    public NotificationType Type => NotificationType.Push;

    private readonly ILogger<ResiliencePushSender> _logger;
    private readonly IAsyncPolicy _asyncPolicy;

    public ResiliencePushSender(ILogger<ResiliencePushSender> logger,
        IAsyncPolicy asyncPolicy)
    {
        _logger = logger;
        _asyncPolicy = asyncPolicy;
    }

    public async Task SendAsync(NotificationRequest notificationRequest, bool forceFail = false, CancellationToken cancellationToken = default)
    {
        await _asyncPolicy
            .ExecuteAsync(async ct =>
            {
                await Task.Delay(Random.Shared.Next(200, 800), ct);

                if (forceFail)
                {
                    throw new Exception("Push provider failed!");
                }


                _logger.ProviderSuccess(Type, notificationRequest.Message);
            }, cancellationToken);
    }
}