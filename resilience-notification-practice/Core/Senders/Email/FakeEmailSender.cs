using resilience_notification_practice.Core.Models;
using resilience_notification_practice.Core.Models.Enums;
using resilience_notification_practice.Infrastructure.Log;

namespace resilience_notification_practice.Core.Senders.Email;

public sealed class FakeEmailSender : IEmailSender
{
    public NotificationType Type => NotificationType.Email;


    private readonly ILogger<FakeEmailSender> _logger;

    public FakeEmailSender(ILogger<FakeEmailSender> logger)
    {
        _logger = logger;
    }



    public async Task SendAsync(NotificationRequest notificationRequest, bool forceFail = false, CancellationToken cancellationToken = default)
    {
        await Task.Delay(Random.Shared.Next(200, 800), cancellationToken);

        if (forceFail)
        {
            throw new Exception("Email provider failed!");
        }


        _logger.ProviderSuccess(Type, notificationRequest.Message);
    }
}