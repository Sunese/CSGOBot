using CSGOBot.Data.Models;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;
using Repository.DbContexts;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace CSGOBot.Modules;

public class FaceitInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ILogger<FaceitInteractionModule> _logger;
    private readonly FaceitService _faceit;
    
    public FaceitInteractionModule(ILogger<FaceitInteractionModule> logger, FaceitService faceit)
    {
        _logger = logger;
        _faceit = faceit;
    }

    [UserCommand("faceitinfo")]
    public async Task FaceitPlayerInfo(IUser user)
    {
        await DeferAsync();
        await using var context = new CsgoBotDataContext();
        var dbUser = await context.FindAsync<User>(user.Id);

        // user does not exist
        if (dbUser == null)
        {
            Log.Logger.Error("");
            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username} is not registered")
                .WithDescription($"Discord user {user.Username} does not exist in database. They should execute the /register faceit command to register.")
                .WithColor(Color.Red)
                .Build();
            await FollowupAsync(embed: embed);
        }

        // user exists but faceit is not registered
        else if (dbUser is { FaceitPlayerId: null })
        {

            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username}'s Faceit info is not registered")
                .WithDescription($"Discord user {user.Username}'s Faceit info does not exist in database. They should execute the /register faceit command to register.")
                .WithColor(Color.Red)
                .Build();
            await FollowupAsync(embed: embed);
        } 

        // user and faceit info exists
        else  if (dbUser.FaceitPlayerId != null)
        {
            _logger.LogDebug($"Faceit id {dbUser.FaceitPlayerId} info was requested for Discord user ID {user.Id}.");
            var faceitPlayer  = await _faceit.GetPlayerInfo(dbUser.FaceitPlayerId);

            var embed = new EmbedBuilder()
                .WithTitle($"{faceitPlayer.nickname}")
                //.WithAuthor(user.Username)
                .WithThumbnailUrl(faceitPlayer.avatar)
                .WithDescription($"{user.Username}'s Faceit profile\nFaceit level: {faceitPlayer.games.csgo.skill_level}\n Faceit elo: {faceitPlayer.games.csgo.faceit_elo}")
                .WithColor(Color.Orange)
                .WithUrl($"https://www.faceit.com/en/players/{faceitPlayer.nickname}")
                .Build();

            await FollowupAsync(embed: embed);
        }
    }
}