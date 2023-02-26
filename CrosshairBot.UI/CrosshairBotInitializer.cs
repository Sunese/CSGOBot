using CrosshairBot.Core.SlashCommands;
using CrosshairBot.Domain.SlashCommands;
using CrosshairBot.Domain.SlashCommands.Handlers;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic.CompilerServices;
using Newtonsoft.Json;

namespace CrosshairBot.Application.Services
{
    public class CrosshairBotInitializer
    {
        private readonly ILogger<CrosshairBotInitializer> logger;
        private readonly DiscordSocketClient client;
        private readonly ISlashCommands slashCommands;


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
                // TODO: register commands
                foreach (var command in await slashCommands.Get())
                {
                    
                }
            };
        }

        public static Task RegisterSlashCommands(DiscordSocketClient client, ILogger<CrosshairBotInitializer> logger)
        {

        }

        
    }
}