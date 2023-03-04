using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace CSGOBot.Modules;

// Interaction modules must be public and inherit from an IInteractionModuleBase
public class CommandModule : ModuleBase<SocketCommandContext>
{
    // Nothing here for now...
    // Consider if telling user to use slash commands when they send some !... command

    //[Command("ping")]
    //[Alias("pong", "hello")]
    //public Task PingAsync()
    //    => ReplyAsync("pong from Text Command Module!");

    //// Ban a user
    //[Command("ban")]
    //[RequireContext(ContextType.Guild, ErrorMessage = "Sorry, this command must be ran from within a server, not a DM!")]
    //// make sure the user invoking the command can ban
    //[RequireUserPermission(GuildPermission.BanMembers, ErrorMessage = "Sorry, you don't have the permission to ban users!")]
    //// make sure the bot itself can ban
    //[RequireBotPermission(GuildPermission.BanMembers, ErrorMessage = "Sorry, I don't have the permission to ban users!")]
    //public async Task BanUserAsync(IGuildUser user, [Remainder] string reason = null)
    //{
    //    await user.Guild.AddBanAsync(user, reason: reason);
    //    await ReplyAsync("ok!");
    //}
}