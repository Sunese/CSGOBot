using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using MediatR;

namespace CrosshairBot.SlashCommands
{
    public enum SlashCommandsEnum
    {
        hello,
        crosshairoftheweek
    };

    public class SlashCommand : IRequest<Action<MessageProperties>>
    {
        public SlashCommandsEnum CommandType { get; set; }
        public SocketSlashCommand SocketSlashCommand { get; set; }

        public SlashCommand()
        {
        }
    }
}
