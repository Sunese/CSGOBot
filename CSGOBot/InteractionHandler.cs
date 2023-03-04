using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CSGOBot;
using CSGOBot.Services;
using Discord.Commands;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using IResult = Discord.Commands.IResult;

namespace InteractionFramework;

public class InteractionHandler
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _handler;
    private readonly IServiceProvider _services;
    private readonly IConfiguration _configuration;
    private readonly CommandService _commands;
    private readonly ILogger<InteractionHandler> _logger;
    private readonly FaceitService _faceit;

    public InteractionHandler(
        DiscordSocketClient client, 
        InteractionService handler, 
        IServiceProvider services, 
        IConfiguration config, 
        CommandService commands, 
        ILogger<InteractionHandler> logger,
        FaceitService faceit)
    {
        _client = client;
        _handler = handler;
        _services = services;
        _configuration = config;
        _commands = commands;
        _logger = logger;
        _faceit = faceit;
    }

    public async Task InitializeAsync()
    {
        // Process when the client is ready, so we can register our commands.
        _client.Ready += ReadyAsync;
        _handler.Log += LogAsync;

        // Add the public modules that inherit InteractionModuleBase<T> to the InteractionService (for modern commands, e.g. slash/user/message)
        await _handler.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        // Register modules that are public and inherit ModuleBase<T>. (for traditional commands, e.g. !help)
        await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);

        // Process the InteractionCreated payloads to execute Interactions commands
        // --> Interaction commands = slash commands, user commands (right click user -> apps), text commands (right click commands -> apps)
        _client.InteractionCreated += HandleInteraction;

        // Process received messages
        _client.MessageReceived += HandleMessageReceived;
        _commands.CommandExecuted += HandleCommandExecuted;

        // Process modal (only used for faceit registering for now)
        _client.ModalSubmitted += HandleModalSubmitted;

    }

    private Task LogAsync(LogMessage message)
    {
        var severity = message.Severity switch
        {
            LogSeverity.Critical => LogEventLevel.Fatal,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Info => LogEventLevel.Information,
            LogSeverity.Verbose => LogEventLevel.Verbose,
            LogSeverity.Debug => LogEventLevel.Debug,
            _ => LogEventLevel.Information
        };

        Log.Write(severity, message.Exception, "[{Source}] {Message}", message.Source, message.Message);

        return Task.CompletedTask;
    }

    private async Task ReadyAsync()
    {
        // Context & Slash commands can be automatically registered, but this process needs to happen after the client enters the READY state.
        // Since Global Commands take around 1 hour to register, we should use a test guild to instantly update and test our commands.
        if (Program.IsDebug())
            foreach (var guild in _client.Guilds)
            {
                await _handler.RegisterCommandsToGuildAsync(guild.Id, true);
            }
        else
            await _handler.RegisterCommandsGloballyAsync(true);
    }

    private async Task HandleInteraction(SocketInteraction interaction)
    {
        try
        {
            // Create an execution context that matches the generic type parameter of your InteractionModuleBase<T> modules.
            var context = new SocketInteractionContext(_client, interaction);

            // Execute the incoming command.
            var result = await _handler.ExecuteCommandAsync(context, _services);

            if (!result.IsSuccess)
                switch (result.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // implement
                        break;
                    default:
                        break;
                }
        }
        catch
        {
            // If Slash Command execution fails it is most likely that the original interaction acknowledgement will persist. It is a good idea to delete the original
            // response, or at least let the user know that something went wrong during the command execution.
            if (interaction.Type is InteractionType.ApplicationCommand)
                await interaction.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
        }
    }

    private async Task HandleMessageReceived(SocketMessage rawMessage)
    {
        // Ignore system messages, or messages from other bots
        if (!(rawMessage is SocketUserMessage message))
            return;
        if (message.Source != MessageSource.User)
            return;

        var argPos = 0;

        var context = new SocketCommandContext(_client, message);

        if (!message.HasCharPrefix('.', ref argPos))
            return;

        // Perform the execution of the command. In this method,
        // the command service will perform precondition and parsing check
        // then execute the command if one is matched.
        await _commands.ExecuteAsync(context, argPos, _services);
        // Note that normally a result will be returned by this format, but here
        // we will handle the result in CommandExecutedAsync,
    }

    public async Task HandleCommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        // command is unspecified when there was a search failure (command not found); we don't care about these errors
        if (!command.IsSpecified)
            return;

        // the command was successful, we don't care about this result, unless we want to log that a command succeeded.
        if (result.IsSuccess)
            return;

        // the command failed, let's notify the user that something happened.
        await context.Channel.SendMessageAsync($"error: {result}");
    }

    public async Task HandleModalSubmitted(SocketModal rawModal)
    {
        if (rawModal.Data.CustomId != "register_faceit")
        {
            // do nothing ... 
            await rawModal.RespondAsync("Did not recognize form. None of your data has been saved :handshake:");
        }

        try
        {
            await rawModal.DeferAsync();
            await rawModal.FollowupAsync("Locating Faceit user and linking to your Discord profile... :mag:");
            await _faceit.RegisterUser(rawModal);
        }
        catch (FaceitService.FaceitServiceException e)
        {
            await rawModal.RespondAsync(e.Message + " :warning:");
        }
    }
}