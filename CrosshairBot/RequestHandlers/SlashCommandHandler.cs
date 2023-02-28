using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrosshairBot.SlashCommands;
using Discord;
using MediatR;

namespace CrosshairBot.RequestHandlers
{
    public class SlashCommandHandler : IRequestHandler<SlashCommand, Action<MessageProperties>>
    {
        public async Task<Action<MessageProperties>> Handle(
            SlashCommand command,
            CancellationToken cancellationToken)
        {
            switch (command.CommandType)
            {
                case SlashCommandsEnum.hello:
                    return await new HelloCommand().MessageAction(command.SocketSlashCommand);
                default:
                    throw new ArgumentException("Unrecognized slash command");
            }
        }
    }
}
