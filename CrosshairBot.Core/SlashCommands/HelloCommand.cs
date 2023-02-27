using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Discord;
using CrosshairBot.Domain.SlashCommands;

namespace CrosshairBot.Core.SlashCommands;

public class HelloCommand : ISlashCommand
{

    /// <summary>
    /// Gets the non-built Hello SlashCommandBuilder
    /// </summary>
    /// <returns></returns>
    public SlashCommandProperties Get()
    {
        var helloCommand = new SlashCommandBuilder();
        // IMPORTANT: Slash command names must be all lowercase!
        helloCommand.WithName("hello");
        helloCommand.WithDescription("This is my first slash command!");
        return helloCommand.Build();
    }

    public async Task Respond(SocketSlashCommand command)
    {
        var embedBuilder = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("This is the hello command!")
            .WithDescription(":)")
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        await command.ModifyOriginalResponseAsync(x => x.Embed = embedBuilder.Build());
    }

    
}
