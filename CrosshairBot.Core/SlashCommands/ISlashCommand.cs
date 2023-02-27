using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CrosshairBot.Domain.SlashCommands;


public interface ISlashCommand
{
    public SlashCommandProperties Get();

}
