using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CSGOBot.Data.Models;
using CSGOBot.Enums;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;
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
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("FaceitApiKey"));
        var response = await _httpClient.GetAsync($"https://open.faceit.com/data/v4/players?nickname={modalFaceitUsername}");

        if (!response.IsSuccessStatusCode)
        {
            throw new FaceitServiceException(
                $"No player with nickname '{modalFaceitUsername}' was found. Please note that this service is CASE-SENSITIVE and double-check the given username.");
        }

        var faceitPlayer = JsonConvert.DeserializeObject<FaceitPlayer>(await response.Content.ReadAsStringAsync());

        if (faceitPlayer is null)
        {
            throw new FaceitServiceException($"Deserializing Faceit Player response from FaceIt API resulted in null");
        }

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
                FaceitPlayerId = faceitPlayer.player_id,
                SteamID64 = faceitPlayer.steam_id_64
            };
            await context.Users.AddAsync(newUser);
        }
        if (user is not null) // user exists - even if faceit info already exists, just replace it
        {
            // TODO: should user be allowed to update? --> for now let's allow it
            user.FaceitPlayerId = faceitPlayer.player_id;
            user.SteamID64 = faceitPlayer.steam_id_64;
            context.Users.Update(user);
        }

        // Also store FaceitPlayer data (if it doesn't exist)
        if (!await context.FaceitPlayers.ContainsAsync(faceitPlayer))
        {
            context.FaceitPlayers.Add(faceitPlayer);
        }
        await context.SaveChangesAsync();
        var embed = new EmbedBuilder()
            .WithTitle("Success!")
            .WithDescription($"Faceit account: {modalFaceitUsername} and Steam account: {faceitPlayer.steam_id_64} has been linked to your discord account!")
            .WithAuthor(modal.User)
            .Build();
        await modal.FollowupAsync(embed: embed);
    }

    
    public class FaceitServiceException : Exception
    {
        public FaceitServiceException()
        {
        }

        public FaceitServiceException(string message)
            : base(message)
        {
        }

        public FaceitServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }


}