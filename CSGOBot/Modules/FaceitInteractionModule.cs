﻿using System;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using CSGOBot.Data.Models;
using CSGOBot.Enums;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;
using Repository.DbContexts;

namespace CSGOBot.Modules;

// Interaction modules must be public and inherit from an IInteractionModuleBase
public class FaceitInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    // Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
    public InteractionService Commands { get; set; }

    private readonly InteractionHandler _handler;
    private readonly FaceitService _faceit;
    //private readonly HltvApiService _hltvApiService;
    //private readonly ProSettingsScraperService _proSettingsScraperService;
        

    // Constructor injection is also a valid way to access the dependencies
    public FaceitInteractionModule(
        InteractionHandler handler,
        FaceitService faceit)
    {
        _handler = handler;
        _faceit = faceit;
        //_hltvApiService = hltvApiService;
        //_proSettingsScraperService = proSettingsScraperService;
    }

    [UserCommand("faceitinfo")]
    public async Task FaceitPlayerInfo(IUser user)
    // todo: add choice for every user in guild that has registered (and 'myself')
    // maybe a bad idea? i believe arguments for slash commands are limited to like 20-30
    // maybe allow command user to mention a user that they want commands for
    {
        await DeferAsync();
        await using var context = new CsgoBotDataContext();
        var dbUser = await context.FindAsync<User>(user.Id);

        // user does not exist
        if (dbUser == null)
        {
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