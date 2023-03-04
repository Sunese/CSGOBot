using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using CSGOBot.Enums;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.DbContexts;

namespace CSGOBot.Services;

public class FaceitService
{
    private readonly ILogger<HltvApiService> _logger;
    private readonly HttpClient _httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    public FaceitService(ILogger<HltvApiService> logger)
    {
        _logger = logger;
    }

    public async Task RegisterUser(SocketModal modal)
    {
        var components = modal.Data.Components.ToList();
        // TODO: prevent SQL injection
        var modalFaceitUsername = components.First(x => x.CustomId == "faceit_username").Value;

        // TODO: ask faceit API if user exists

        await using var context = new CsgoBotDataContext();
        // TODO: check if 1) the user exists and 2) faceit column is null
        var user = await context.Users.FindAsync(modal.User.Id);
        if (user is null)
        {
            // add user and recurse 
            var newUser = new User()
            {
                Id = modal.User.Id,
                Name = modal.User.Username,
                FaceitPlayerId = null,
                SteamID64 = null
            };
            await context.Users.AddAsync(newUser);
            await context.SaveChangesAsync();
            // I believe this returns, so real recursion should be happening
            // TODO: test that this actually returns here
            // (i.e. does not re-run RegisterUser in another thread while also continuing execution from here on same thread)
            await RegisterUser(modal);
        }
        if (user is not null) // user exists - even if faceit info already exists, just replace it
        {
            // TODO: should user be allowed to update? --> for now let's allow it
            user.FaceitPlayerId = modalFaceitUsername;
            context.Users.Update(user);
        }
        await context.SaveChangesAsync();
        var embed = new EmbedBuilder()
            .WithTitle("Success!")
            .WithDescription($"Faceit account: {modalFaceitUsername} has been linked to your discord account!")
            .WithAuthor(modal.User)
            .Build();
        //await notification.Modal.ModifyOriginalResponseAsync(x => x.Content = "Faceit account has been linked to your discord account!");
        await modal.RespondAsync(embed: embed);
    }
}