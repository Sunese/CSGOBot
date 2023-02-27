using System;
using System.Threading.Tasks;
using CrosshairBot.Core.SlashCommands;
using CrosshairBot.Application;
using CrosshairBot.Domain.SlashCommands;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.UI
{
    public static class CrosshairBotInitializer
    {
        // static async method that behaves like a constructor       
        public static async Task Initialize(
            DiscordSocketClient client,
            ILogger<CrosshairBotInitializer> logger,
            ILogger<SlashCommandHandler> slashCommandHandlerLogger)
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

                var helloCommand = new HelloCommand().Get();

                // initialize + build more commands here
                // IMPORTANT: Slash command names must be all lowercase!
                foreach (var guild in client.Guilds)
                {
                    // TODO: this is for debugging, remove
                    await guild.DeleteApplicationCommandsAsync();
                    await guild.CreateApplicationCommandAsync(helloCommand);
                    //await guild.create...
                }
                logger.LogInformation("Done registering slash commands. Waiting for clients...");
            };

            var slashCommandHandler = new SlashCommandHandler(slashCommandHandlerLogger);

            client.SlashCommandExecuted += slashCommandHandler.Handle;
        }


    }
}