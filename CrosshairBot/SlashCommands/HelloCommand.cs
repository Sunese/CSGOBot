using Discord;
using Discord.WebSocket;
using Microsoft.AspNetCore.Http;

namespace CrosshairBot.SlashCommands;

public class HelloCommand
{
    public SlashCommandProperties SlashCommandProperties;

    public HelloCommand()
    {
        var helloCommand = new SlashCommandBuilder();
        // IMPORTANT: Slash command names must be all lowercase!
        helloCommand.WithName("hello");
        helloCommand.WithDescription("This is my first slash command!");
        SlashCommandProperties = helloCommand.Build();
    }

    public async Task<Action<MessageProperties>> MessageAction(SocketSlashCommand command)
    {
        var embedBuilder = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("This is the hello command!")
            .WithDescription(":)")
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        return await Task.FromResult(
            new Action<MessageProperties>(
                x => x.Embed = embedBuilder.Build()));
    }


}
