using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CrosshairBot.Notifications;
using CrosshairBot.SlashCommands;

namespace CrosshairBot.AutoMapper.Resolvers
{
    public class CommandTypeResolver
        : IValueResolver<SlashCommandExecutedNotification, SlashCommand, SlashCommandsEnum>
    {
        public SlashCommandsEnum Resolve(
            SlashCommandExecutedNotification source,
            SlashCommand destination,
            SlashCommandsEnum destMember,
            ResolutionContext context)
        {
            return Enum.Parse<SlashCommandsEnum>(source.Command.CommandName);
        }
    }
}
