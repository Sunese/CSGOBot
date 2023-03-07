using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CSGOBot.Data.Models;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.DbContexts;

namespace CSGOBot.Services;

public class SteamService
{
    private readonly ILogger<SteamService> _logger;
    private readonly HttpClient _httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    public SteamService(ILogger<SteamService> logger)
    {
        _logger = logger;
    }

    public async Task<Player> RegisterUserAsync(SocketUser discordUser, string steamid64)
    {
        var steamApiKey = Environment.GetEnvironmentVariable("SteamApiKey");

        var response = await _httpClient.GetAsync($"http://api.steampowered.com/ISteamUser/GetPlayerSummaries/v0002/?key={steamApiKey}&steamids={steamid64}");

        if (!response.IsSuccessStatusCode)
        {
            //throw new FaceitServiceException(
            //    $"No player with nickname '{modalFaceitUsername}' was found. Please note that this service is CASE-SENSITIVE and double-check the given username.");
            throw new SteamServiceException("An error with SteamAPI occurred.");
        }

        var playerSummaries = JsonConvert.DeserializeObject<SteamApiResponse>(await response.Content.ReadAsStringAsync());

        if (playerSummaries is null)
        {
            throw new SteamServiceException($"Deserializing response from Steam API resulted in null");
        }

        await using var context = new CsgoBotDataContext();
        var user = await context.Users.FindAsync(discordUser.Id);
        if (user is null)
        {
            var newUser = new User()
            {
                Id = discordUser.Id,
                Name = discordUser.Username,
                FaceitPlayerId = null,
                SteamID64 = playerSummaries.response.players.First().steamid
            };
            await context.Users.AddAsync(newUser);
        }
        if (user is not null) // user exists - even if steam info already exists, just replace it
        {
            // TODO: should user be allowed to update? --> for now let's allow it
            user.SteamID64 = playerSummaries.response.players.First().steamid;
            context.Users.Update(user);
        }

        if (await context.SaveChangesAsync() > 0)
        {
            return playerSummaries.response.players.First();
        }
        else
        {
            throw new SteamServiceException($"No data was saved to database when registering Steam ID {steamid64} to Discord ID {discordUser.Id}");
        }
    }

    //public async Task<FaceitPlayer> GetPlayerInfo(Guid? faceitPlayerId)
    //{

    //}

    
    public class SteamServiceException : Exception
    {
        public SteamServiceException()
        {
        }

        public SteamServiceException(string message)
            : base(message)
        {
        }

        public SteamServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }


}