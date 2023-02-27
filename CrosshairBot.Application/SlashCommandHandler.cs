using System;
using CrosshairBot.Core.SlashCommands;
using Discord.WebSocket;
using CrosshairBot.Core.SlashCommands;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.Application
{
	public class SlashCommandHandler
	{
		private ILogger<SlashCommandHandler> _logger;

		public SlashCommandHandler(ILogger<SlashCommandHandler> logger)
		{
			_logger = logger;
		}

		public Task Handle(SocketSlashCommand command)
		{
			// Defer first, then modify original message to avoid timeout from discord which I think is 5 seconds or something
			// This way we have 15 minutes to modifi the message (which is just the bot loading)
			command.DeferAsync();

			switch (command.CommandName)
			{
				case "hello":
					HelloCommandHandler helloCommandHandler = new();
					helloCommandHandler.Handle(command);
					break;
				default:
					command.ModifyOriginalResponseAsync(x => x.Content = "Unrecognized slash command");
					_logger.LogError($"Unrecognized slash command: {command.CommandName}");
					break;
			}
			_logger.LogDebug($"Handled \"{command.CommandName}\" command from user \"{command.User.Username}\" on guild ID \"{command.GuildId}\"");
			return Task.CompletedTask;
		}
	}
}