using Polly;
using resilience_notification_practice.Core.Models;
using resilience_notification_practice.Core.Models.Enums;
using resilience_notification_practice.Infrastructure.Log;

namespace resilience_notification_practice.Core.Senders.Email;

public sealed class ResilienceEmailSender : IEmailSender
{
    public NotificationType Type => NotificationType.Email;


    private readonly ILogger<ResilienceEmailSender> _logger;
    private readonly IAsyncPolicy _asyncPolicy;

    public ResilienceEmailSender(ILogger<ResilienceEmailSender> logger, IAsyncPolicy asyncPolicy)
    {
        _logger = logger;
        _asyncPolicy = asyncPolicy;
    }



    public async Task SendAsync(NotificationRequest notificationRequest, bool forceFail = false, CancellationToken cancellationToken = default)
    {
        await _asyncPolicy.ExecuteAsync(async ct =>
        {
            // simulate delay
            await Task.Delay(Random.Shared.Next(200, 800), ct);

            if (forceFail)
            {
                throw new Exception("Email provider failed!");
            }

            _logger.ProviderSuccess(Type, notificationRequest.Message);

        }, cancellationToken);
    }
}