using CrosshairBot.Core.SlashCommands;
using CrosshairBot.Domain.SlashCommands;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.UI
{
    public class CrosshairBotInitializer
    {
        // static async method that behaves like a constructor       
        public static async Task Initialize(DiscordSocketClient client, ILogger<CrosshairBotInitializer> logger, ISlashCommands slashCommands)
        {
            logger.LogDebug("Initializing bot...");


            var token = Environment.GetEnvironmentVariable("DiscordBotToken");

            await client.LoginAsync(TokenType.Bot, token);
            await client.StartAsync();

            client.Connected += () =>
            {
                logger.LogInformation("Bot is connected");
                return Task.CompletedTask;
            };

            client.Ready += async () =>
            {
                logger.LogInformation("Bot is ready, registering slash commands.");
                foreach (var command in slashCommands.Get())
                {
                    foreach (var guild in client.Guilds)
                    {
                        await guild.CreateApplicationCommandAsync(command);
                    }
                }
                logger.LogInformation("Done registering slash commands.");
            };

            client.SlashCommandExecuted += command =>
            {
                // AutoMapper here... e.g. might resolve to a HelloCommand

                // E.g. we should get an already instantiated HelloCommand from
                var resolvedCommand = ;
            };
        }


    }
}