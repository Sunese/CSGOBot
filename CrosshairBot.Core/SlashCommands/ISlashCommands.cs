using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Discord;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace CrosshairBot.Domain.SlashCommands;


public interface ISlashCommands
{
    public List<IApplicationCommand> Get();
    public void Add();
    //public Task Remove();
    //public Task Update();
}
