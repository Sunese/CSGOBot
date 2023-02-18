using CrosshairBot.Domain.SlashCommands.Handlers;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord;

namespace CrosshairBot.Domain.SlashCommands;

public enum SlashCommandsEnum
{
    hello
}

public class SlashCommands : ISlashCommands
{
    private ILogger<SlashCommands> logger;
    private DiscordSocketClient client;
    private List<SlashCommandBuilder> commands;

    public SlashCommands(ILogger<SlashCommands> logger, DiscordSocketClient client)
    {
        this.logger = logger;
        this.client = client;
    }

    public async Task SetSlashCommands(List<SlashCommandBuilder> commands)
    {
        foreach (var command in commands)
        {
            var guild = client.Guilds.FirstOrDefault();
            await guild.CreateApplicationCommandAsync(command.Build());
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
            default:
                throw new Exception("Undefined command");
        }
    }
}
