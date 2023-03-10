using CSGOBot.Data.Models;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;
using Repository.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CSGOBot.Modules;

public class SteamInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ILogger<SteamInteractionModule> _logger;
    private readonly SteamService _steam;
    
    public SteamInteractionModule(ILogger<SteamInteractionModule> logger, SteamService steam)
    {
        _logger = logger;
        _steam = steam;
    }


    [UserCommand("steaminfo")]
    public async Task SteamAccountInfo(IUser user)
    {
        await DeferAsync();
        await using var context = new CsgoBotDataContext();

        var dbUser = await context.FindAsync<User>(user.Id);

        // user does not exist at all
        if (dbUser == null)
        {
            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username} is not registered")
                .WithDescription($"Discord user {user.Username} does not exist in database. They should execute the /register steam command to register.")
                .WithColor(Color.Red)
                .Build();
            await FollowupAsync(embed: embed);
        }

        // user exists but no steam info
        else if (dbUser is { SteamID64: null })
        {
            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username}'s Steam info is not registered")
                .WithDescription($"Discord user {user.Username}'s Steam info does not exist in database. They should execute the /register steam command to register.")
                .WithColor(Color.Red)
                .Build();
            await FollowupAsync(embed: embed);
        }

        // user and steam info exists
        else if (dbUser.SteamID64 != null)
        {
            var steamAccount = await _steam.GetAccountInfo(dbUser.SteamID64);

            var embed = new EmbedBuilder()
                .WithTitle($"{steamAccount.personaname}")
                //.WithAuthor(user.Username)
                .WithThumbnailUrl(steamAccount.avatarfull)
                .WithDescription($"{user.Username}'s Steam profile")
                .WithColor(Color.Blue)
                .WithUrl(steamAccount.profileurl)
                .Build();

            await FollowupAsync(embed: embed);
        }
    }
}
