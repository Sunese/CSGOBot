using Discord;
using Discord.WebSocket;
using MediatR;

namespace CrosshairBot.Notifications;

public class SlashCommandExecutedNotification : INotification
{
    public SlashCommandExecutedNotification(SocketSlashCommand cmd)
    {
        Command = cmd ?? throw new ArgumentNullException(nameof(cmd));
    }

    public SocketSlashCommand Command { get; }
}