using CSGOBot.Data.Models;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;
using Repository.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOBot.Modules;

public class SteamInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    // Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
    public InteractionService Commands { get; set; }

    private readonly InteractionHandler _handler;
    private readonly SteamService _steam;

    private readonly FaceitService _faceit;
    //private readonly HltvApiService _hltvApiService;
    //private readonly ProSettingsScraperService _proSettingsScraperService;


    // Constructor injection is also a valid way to access the dependencies
    public SteamInteractionModule(
        InteractionHandler handler,
        SteamService steam,
        FaceitService faceit)
    {
        _handler = handler;
        _steam = steam;
        _faceit = faceit;
        //_faceit = faceit;
        //_hltvApiService = hltvApiService;
        //_proSettingsScraperService = proSettingsScraperService;
    }


    [UserCommand("steaminfo")]
    public async Task SteamAccountInfo(IUser user)
        // todo: add choice for every user in guild that has registered (and 'myself')
        // maybe a bad idea? i believe arguments for slash commands are limited to like 20-30
        // maybe allow command user to mention a user that they want commands for
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
