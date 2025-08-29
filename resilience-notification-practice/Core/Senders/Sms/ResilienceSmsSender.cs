using Polly;
using resilience_notification_practice.Core.Models;
using resilience_notification_practice.Core.Models.Enums;
using resilience_notification_practice.Infrastructure.Log;

namespace resilience_notification_practice.Core.Senders.Sms;

public sealed class ResilienceSmsSender : ISmsSender
{
    public NotificationType Type => NotificationType.Sms;

    private readonly ILogger<ResilienceSmsSender> _logger;
    private readonly IAsyncPolicy _asyncPolicy;

    public ResilienceSmsSender(ILogger<ResilienceSmsSender> logger, IAsyncPolicy asyncPolicy)
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
                    throw new Exception("Sms provider failed!");
                }


                _logger.ProviderSuccess(Type, notificationRequest.Message);
            }, cancellationToken);
    }
}