using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace CrosshairBot.Domain.SlashCommands.Handlers
{
    public class DefaultCommandsHandler
    {
        public static async Task NotRecognizedCommand(SocketSlashCommand command)
        {
            var embedBuilder = new EmbedBuilder()
                .WithAuthor(command.User)
                .WithTitle("Unrecognized command \x1F349")
                .WithColor(Color.Green)
                .WithCurrentTimestamp();

            await command.RespondAsync(embed: embedBuilder.Build());
        }
    }
}
