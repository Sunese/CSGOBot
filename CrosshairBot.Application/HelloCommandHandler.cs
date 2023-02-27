using System;
using CrosshairBot.Core.SlashCommands;
using Discord.WebSocket;
using CrosshairBot.Core.SlashCommands;

namespace CrosshairBot.Application
{
	public class HelloCommandHandler
	{
		public HelloCommandHandler()
		{
			
		}

		public void Handle(SocketSlashCommand command)
		{
			HelloCommand.Respond(command);
		}
	}
}

