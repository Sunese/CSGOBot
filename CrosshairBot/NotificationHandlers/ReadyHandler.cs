using CrosshairBot.Notifications;
using CrosshairBot.RequestHandlers;
using CrosshairBot.SlashCommands;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.NotificationHandlers;

public class ReadyHandler : INotificationHandler<ReadyNotification>
{
    private readonly ILogger<ReadyHandler> _logger;
    private readonly DiscordSocketClient _client;

   public ReadyHandler(ILogger<ReadyHandler> logger, DiscordSocketClient client)
    {
        _logger = logger;
        _client = client;
    }

    public async Task Handle(ReadyNotification notification, CancellationToken cancellationToken)
    {

        // Your implementation
        _logger.LogInformation("Bot is ready, registering slash commands.");

        var helloCommand = new HelloCommand().SlashCommandProperties;
        var crosshairOfTheWeekCommand = new CrosshairOfTheWeekCommand().SlashCommandProperties;

        // initialize + build more commands here
        // IMPORTANT: Slash command names must be all lowercase!
        foreach (var guild in _client.Guilds)
        {
            // TODO: this is for debugging, remove
            await guild.DeleteApplicationCommandsAsync();
            await guild.CreateApplicationCommandAsync(helloCommand);
            await guild.CreateApplicationCommandAsync(crosshairOfTheWeekCommand);
            //await guild.create...
        }
        _logger.LogInformation("Done registering slash commands. Waiting for clients...");
    }
}