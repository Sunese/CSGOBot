using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.Domain.SlashCommands.Handlers;

public interface ICrosshairCommandsHandler
{
    public Task Respond(SocketSlashCommand command);

}
