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

public class BettingInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly ILogger<BettingInteractionModule> _logger;
    private readonly UserService _user;
    private readonly BettingService _betting;
    
    public BettingInteractionModule(ILogger<BettingInteractionModule> logger, UserService user, BettingService betting)
    {
        _logger = logger;
        _user = user;
        _betting = betting;
    }

    [UserCommand("bettingbalance")]
    public async Task GetBettingBalanceAsync(IUser user)
    {
        await DeferAsync();

        if (!await _user.ExistsAsync(user.Id))
        {
            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username} is not registered")
                .WithDescription($"Discord user {user.Username} does not exist in database. They should execute one of the /register commands to register.")
                .WithColor(Color.Red)
                .Build();
            await FollowupAsync(embed: embed);
        }
        else
        {
            var balance = await _betting.GetBettingBalanceAsync(user.Id);
            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username} has ${balance} in betting balance!")
                //.WithDescription($"Discord user {user.Username} does not exist in database. They should execute the /register steam command to register.")
                .WithColor(Color.Green)
                .WithAuthor(user)
                .WithCurrentTimestamp()
                .Build();
            await FollowupAsync(embed: embed);
        }
    }


}
