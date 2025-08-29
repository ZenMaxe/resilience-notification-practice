using Polly;

namespace resilience_notification_practice.Infrastructure.Resilience;

public static class NotificationPollyPolicies
{
    private const string PolicyKey = "DefaultPolicy";

    public static IAsyncPolicy CreateDefaultPolicy(
        ILogger logger)
    {
        var retry = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 2,
                sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(200),
                onRetry: (exception, timeSpan, attempt, context) =>
                {
                    logger.LogWarning(exception, "[Retry-{Key}] Attempt {Attempt} failed after {Delay}ms | CorrelationId: {CorrelationId}",
                        PolicyKey, attempt, timeSpan.TotalMilliseconds, context.CorrelationId);
                });

        var timeout = Policy.TimeoutAsync(2); // Timeout after 2 seconds


        var circuitBreaker = Policy
            .Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromSeconds(10),
                onBreak: (ex, breakTime) =>
                {
                    logger.LogError(ex, "[CircuitBreaker-{Key}] Circuit broken for {BreakTime}s due to: {Message}",
                        PolicyKey, breakTime.TotalSeconds, ex.Message);
                },
                onReset: () =>
                {
                    logger.LogInformation("[CircuitBreaker-{Key}] Circuit reset", PolicyKey);
                },
                onHalfOpen: () => {});


        return Policy.WrapAsync(retry, timeout, circuitBreaker);
    }
}