using CrosshairBot.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.NotificationHandlers;

public class MessageReceivedHandler : INotificationHandler<MessageReceivedNotification>
{

    private readonly ILogger<MessageReceivedHandler> _logger;

    public MessageReceivedHandler(ILogger<MessageReceivedHandler> logger)
    {
        _logger = logger;
    }

    public async Task Handle(MessageReceivedNotification notification, CancellationToken cancellationToken)
    {
        //Console.WriteLine($"MediatR works! (Received a message by {notification.Message.Author.Username})");
        _logger.LogDebug($"Received a message from {notification.Message.Author.Username}");

        // Your implementation
    }
}