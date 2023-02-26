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


namespace CrosshairBot.Core.SlashCommands;

public class HelloCommand
{
    private readonly SlashCommandBuilder _commandBuilder;

    public HelloCommand()
    {
        var helloCommand = new SlashCommandBuilder();
        helloCommand.WithName("hello");
        helloCommand.WithDescription("This is my first slash command!");
        _commandBuilder = helloCommand;
    }

    public SlashCommandProperties Build()
    {
        return _commandBuilder.Build();
    }

    public static async Task Respond(SocketSlashCommand command)
    {
        var embedBuilder = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("This is the hello command!")
            .WithDescription(":)")
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        await command.RespondAsync(embed: embedBuilder.Build());
    }

    
}
