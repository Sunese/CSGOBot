using CrosshairBot.Domain.SlashCommands.Handlers;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord;
using CrosshairBot.Core.SlashCommands;
using System.Collections.Generic;

namespace CrosshairBot.Domain.SlashCommands;



public class SlashCommands : ISlashCommands
{
    private ILogger<SlashCommands> logger;
    private DiscordSocketClient client;
    private ICrosshairCommandsHandler crosshairCommandsHandler;

    private List<IApplicationCommand> commands = new()
    {
        new HelloCommand()
    };

    //public enum SlashCommandsEnum
    //{
    //    hello,
    //    crosshair
    //}

    //public enum CrosshairCommandsEnum
    //{
    //    crosshairOfTheDay,
    //    crosshairOfTheWeek,
    //    crosshiarOfTheMonth,
    //    crosshairOfTheYear,
    //    crosshairOfPlayer
    //}

    //public SlashCommands(ILogger<SlashCommands> logger, DiscordSocketClient client, ICrosshairCommandsHandler crosshairCommandsHandler)
    //{
    //    this.logger = logger;
    //    this.client = client;
    //    this.crosshairCommandsHandler = crosshairCommandsHandler;
    //}

    //public async Task SetSlashCommands(List<SlashCommandBuilder> commands)
    //{
    //    foreach (var command in commands)
    //    {
    //        foreach (var guild in client.Guilds)
    //        {
    //            await guild.CreateApplicationCommandAsync(command.Build());
    //        }
    //    }
    //}

    //public async Task Handle(SocketSlashCommand command)
    //{
    //    switch (command.Data.Name)
    //    {
    //        // TODO: enum parsing + having some correlation between enum type and what handler to call
    //        case "hello":
    //            await HandleHelloCommands.HandleHelloCommand(command);
    //            logger.LogDebug("Sent hello command!");
    //            break;
    //        case "crosshairoftheweek":
    //            logger.LogDebug("caught xhair command!");
    //            await command.DeferAsync();
    //            await crosshairCommandsHandler.Respond(command);
    //            logger.LogDebug("sent xhair command");
    //            break;
    //        default:
    //            await DefaultCommandsHandler.NotRecognizedCommand(command);
    //            break;
    //    }
    //}

    public List<IApplicationCommand> Get()
    {
        return commands;
    }

    public void Add(IApplicationCommand command)
    {
        commands.Add(command);
    }
}
