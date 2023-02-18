using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.Domain.SlashCommands.Handlers;

public static class HandleHelloCommands
{
    public static async Task HandleHelloCommand(SocketSlashCommand command)
    {
        var embedBuilder = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Hello!")
            .WithDescription("This is a description that says hello!")
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        await command.RespondAsync(embed: embedBuilder.Build());
    }

}
