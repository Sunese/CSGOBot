using CrosshairBot.Domain.SlashCommands.Handlers;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord;

namespace CrosshairBot.Domain.SlashCommands;

public enum SlashCommandsEnum
{
    hello,
    crosshairOfTheDay
}

public class SlashCommands : ISlashCommands
{
    private ILogger<SlashCommands> logger;
    private DiscordSocketClient client;
    private List<SlashCommandBuilder> commands;
    private ICrosshairCommandsHandler crosshairCommandsHandler;

    public SlashCommands(ILogger<SlashCommands> logger, DiscordSocketClient client)
    {
        this.logger = logger;
        this.client = client;
    }

    public async Task SetSlashCommands(List<SlashCommandBuilder> commands)
    {
        foreach (var command in commands)
        {
            foreach (var guild in client.Guilds)
            {
                await guild.CreateApplicationCommandAsync(command.Build());
            }
        }
    }

    public async Task Handle(SocketSlashCommand command)
    {
        switch (command.Data.Name)
        {
            // TODO: enum parsing + having some correlation between enum type and what handler to call
            case "hello":
                await HandleHelloCommands.HandleHelloCommand(command);
                logger.LogDebug("Sent hello command!");
                break;
            case "crosshairoftheweek":
                logger.LogDebug("caught xhair command!");
                await crosshairCommandsHandler.Respond(command);
                logger.LogDebug("sent xhair command");
                break;
            default:
                await DefaultCommandsHandler.NotRecognizedCommand(command);
                break;
        }
    }
}
