using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CrosshairBot.AutoMapper.Resolvers;
using CrosshairBot.Notifications;
using CrosshairBot.SlashCommands;
using Microsoft.AspNetCore.Mvc;


namespace CrosshairBot.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<SlashCommandExecutedNotification, SlashCommand>()
                .ForMember(
                    dest => dest.CommandType,
                    obj => obj.MapFrom<CommandTypeResolver>())
                .ForMember(
                    dest => dest.SocketSlashCommand,
                    obj => obj.MapFrom(src => src.Command));
            // .ForMember(...)
        }
    }
}
