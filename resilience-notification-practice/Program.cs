using Polly;
using resilience_notification_practice.API;
using resilience_notification_practice.Core.Dispatchers;
using resilience_notification_practice.Core.Interfaces;
using resilience_notification_practice.Core.Interfaces.Handlers;
using resilience_notification_practice.Core.Interfaces.Services;
using resilience_notification_practice.Core.Senders.Email;
using resilience_notification_practice.Core.Senders.Push;
using resilience_notification_practice.Core.Senders.Sms;
using resilience_notification_practice.Core.Services;
using resilience_notification_practice.Infrastructure.Resilience;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace resilience_notification_practice;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithThreadId()
            .Enrich.WithProcessId()
            .Enrich.WithEnvironmentName()
            .Enrich.WithCorrelationId()
            .WriteTo.Console(
                outputTemplate:
                "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties}{NewLine}{Exception}")
            .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
            .MinimumLevel.Debug()
            .CreateLogger();


        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseSerilog();
        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();


        builder.Services.AddSingleton<IAsyncPolicy>(sp =>
        {
            var logger = sp.GetRequiredService<ILogger<Program>>();
            return NotificationPollyPolicies.CreateDefaultPolicy(logger);
        });

        builder.Services.AddSingleton<ISender, ResilienceEmailSender>();
        builder.Services.AddSingleton<ISender, ResilienceSmsSender>();
        builder.Services.AddSingleton<ISender, ResiliencePushSender>();

        builder.Services.AddSingleton<INotificationSenderResolver, NotificationSenderResolver>();
        builder.Services.AddSingleton<INotificationDispatcher, NotificationDispatcher>();



        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapNotificationEndpoints();


        app.Run();
    }
}