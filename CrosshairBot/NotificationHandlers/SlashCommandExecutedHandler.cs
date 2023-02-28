using AutoMapper;
using CrosshairBot.Notifications;
using CrosshairBot.SlashCommands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.NotificationHandlers;

public class SlashCommandExecutedHandler : INotificationHandler<SlashCommandExecutedNotification>
{
    private readonly ILogger<SlashCommandExecutedHandler> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;

    public SlashCommandExecutedHandler(ILogger<SlashCommandExecutedHandler> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    public async Task Handle(SlashCommandExecutedNotification notification, CancellationToken cancellationToken)
    {
        //Console.WriteLine($"MediatR works! (Received a message by {notification.Message.Author.Username})");
        _logger.LogDebug($"Handling a {notification.Command.CommandName} command from {notification.Command.User.Username}");

        await notification.Command.DeferAsync();

        var slashCommand = _mapper.Map<SlashCommandExecutedNotification, SlashCommand>(notification);
        var messagePropertiesAction = await _mediator.Send(slashCommand, cancellationToken);

        await notification.Command.ModifyOriginalResponseAsync(messagePropertiesAction);
        
        _logger.LogDebug($"Handled \"{notification.Command.CommandName}\" command from user \"{notification.Command.User.Username}\" on guild ID \"{notification.Command.GuildId}\"");
    }
}