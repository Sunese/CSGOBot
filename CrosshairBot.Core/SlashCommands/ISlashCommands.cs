using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord;

namespace CrosshairBot.Domain.SlashCommands;


public interface ISlashCommands
{
    public async Task SetSlashCommands(List<SlashCommandBuilder> commands)
    {
    }

    public async Task Handle(SocketSlashCommand command)
    {
    }
}
