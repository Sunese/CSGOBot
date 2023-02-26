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

public class HelloCommand : ApplicationCommandProperties
{
    private readonly ILogger<HelloCommand> logger;

    private readonly SlashCommandProperties _command;


    internal ApplicationCommandType Type => ApplicationCommandType.Slash;

    /// <summary>
    ///    Gets or sets the discription of this command.
    /// </summary>
    public Optional<string> Description { get; set; }

    /// <summary>
    ///     Gets or sets the options for this command.
    /// </summary>
    public Optional<List<ApplicationCommandOptionProperties>> Options { get; set; }


    public HelloCommand()
    {
        var helloCommand = new SlashCommandBuilder();
        helloCommand.WithName("hello");
        helloCommand.WithDescription("This is my first slash command!");
        _command = (ApplicationCommandProperties)helloCommand.Build();
    }

    internal override ApplicationCommandType GetType()
    {
        throw new NotImplementedException();
    }


    public async Task Respond(SocketSlashCommand command)
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
