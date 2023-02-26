using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.Core.SlashCommands;

public class HelloCommand : IApplicationCommand
{
    private readonly ILogger<HelloCommand> logger;

    private readonly IApplicationCommand old_command;
    private IApplicationCommand _command;

    public HelloCommand()
    {
        var helloCommand = new SlashCommandBuilder();
        helloCommand.WithName("hello");
        helloCommand.WithDescription("This is my first slash command!");
        _command = (IApplicationCommand)helloCommand.Build();
    }

    public IApplicationCommand Get()
    {
        return _command;
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

    public ulong Id => _command.Id;

    public DateTimeOffset CreatedAt => _command.CreatedAt;

    public Task DeleteAsync(RequestOptions options = null)
    {
        return _command.DeleteAsync(options);
    }

    public Task ModifyAsync(Action<ApplicationCommandProperties> func, RequestOptions options = null)
    {
        return _command.ModifyAsync(func, options);
    }

    public Task ModifyAsync<TArg>(Action<TArg> func, RequestOptions options = null) where TArg : ApplicationCommandProperties
    {
        return _command.ModifyAsync(func, options);
    }

    public ulong ApplicationId => _command.ApplicationId;

    public ApplicationCommandType Type => _command.Type;

    public string Name => _command.Name;

    public string Description => _command.Description;

    public bool IsDefaultPermission => _command.IsDefaultPermission;

    public bool IsEnabledInDm => _command.IsEnabledInDm;

    public bool IsNsfw => _command.IsNsfw;

    public GuildPermissions DefaultMemberPermissions => _command.DefaultMemberPermissions;

    public IReadOnlyCollection<IApplicationCommandOption> Options => _command.Options;

    public IReadOnlyDictionary<string, string> NameLocalizations => _command.NameLocalizations;

    public IReadOnlyDictionary<string, string> DescriptionLocalizations => _command.DescriptionLocalizations;

    public string NameLocalized => _command.NameLocalized;

    public string DescriptionLocalized => _command.DescriptionLocalized;
}
