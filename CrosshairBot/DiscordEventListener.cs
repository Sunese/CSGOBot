using CrosshairBot.Notifications;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CrosshairBot;

public class DiscordEventListener
{
    private readonly CancellationToken _cancellationToken;

    private readonly DiscordSocketClient _client;
    private readonly IServiceScopeFactory _serviceScope;
    private readonly ILogger<DiscordEventListener> _logger;

    public DiscordEventListener(DiscordSocketClient client, IServiceScopeFactory serviceScope, ILogger<DiscordEventListener> logger)
    {
        _client = client;
        _serviceScope = serviceScope;
        _cancellationToken = new CancellationTokenSource().Token;
        _logger = logger;
    }

    private IMediator Mediator
    {
        get
        {
            var scope = _serviceScope.CreateScope();
            return scope.ServiceProvider.GetRequiredService<IMediator>();
        }
    }

    public Task StartAsync()
    {
        _client.Ready += OnReadyAsync;
        _client.MessageReceived += OnMessageReceivedAsync;
        _client.SlashCommandExecuted += OnSlashCommandExecuted;

        return Task.CompletedTask;
    }

    private Task OnSlashCommandExecuted(SocketSlashCommand arg)
    {
        _logger.LogDebug($"Slash command executed: {arg.CommandName} by user {arg.User.Username} from guild ID {arg.GuildId}");

        return Mediator.Publish(new SlashCommandExecutedNotification(arg), _cancellationToken);
    }

    private Task OnMessageReceivedAsync(SocketMessage arg)
    {
        _logger.LogDebug("MessageReceived event triggered: Publishing MessageReceivedNotification.");

        return Mediator.Publish(new MessageReceivedNotification(arg), _cancellationToken);
    }

    private Task OnReadyAsync()
    {
        _logger.LogDebug("Ready event triggered: Publishing ReadyNotification.");
        return Mediator.Publish(ReadyNotification.Default, _cancellationToken);
    }
}