using resilience_notification_practice.Core.Interfaces.Handlers;
using resilience_notification_practice.Core.Models;

namespace resilience_notification_practice.API;

public static class NotificationEndpoints
{
    public static IEndpointRouteBuilder MapNotificationEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var mapGroup = endpoints.MapGroup("api/notifications");


        mapGroup.MapPost("/", CreateNotificationRequest)
            .Accepts<NotificationRequest>("application/json")
            .WithDisplayName("Create Notification")
            .WithSummary("Creates a new Notification Request")
            .WithName("CreateNotificationRequest");


        return endpoints;
    }


    private static async Task<IResult> CreateNotificationRequest(NotificationRequest request, INotificationDispatcher dispatcher,
        CancellationToken cancellationToken)
    {
        await dispatcher.SendNotificationAsync(request, cancellationToken);

        return Results.NoContent();
    }
}