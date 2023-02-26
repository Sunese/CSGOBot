using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord;

namespace CrosshairBot.Domain.SlashCommands;


public interface ISlashCommands
{
    public Task<List<IApplicationCommand>> Get();
}
