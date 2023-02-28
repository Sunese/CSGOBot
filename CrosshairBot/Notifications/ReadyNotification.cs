using MediatR;

namespace CrosshairBot.Notifications;

public class ReadyNotification : INotification
{
    public static readonly ReadyNotification Default
        = new();

    private ReadyNotification()
    {
    }
}