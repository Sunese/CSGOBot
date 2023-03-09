using System;
using System.Linq;
using System.Net;
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

    public async Task<bool> TryRegisterUserAsync(SocketUser discordUser, string faceitUsername)
    {
        bool wasFound;
        // Ask faceit API if user exists
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("FaceitApiKey"));
        var response = await _httpClient.GetAsync($"https://open.faceit.com/data/v4/players?nickname={faceitUsername}");

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            wasFound = false;
        }
        // if any else httpstatuscode, throw ex, dont know how to handle this
        else if (!response.IsSuccessStatusCode)
        {
            throw new FaceitServiceException($"Error occurred with Faceit API. Statuscode: {response.StatusCode}. Content: {response.Content}");
        }
        else // we got a success status code
        {
            var faceitPlayer = JsonConvert.DeserializeObject<FaceitPlayer>(await response.Content.ReadAsStringAsync());
            if (faceitPlayer is null)
            {
                throw new FaceitServiceException($"Deserializing Faceit Player response from FaceIt API resulted in null");
            }

            await using var context = new CsgoBotDataContext();
            var user = await context.Users.FindAsync(discordUser.Id);
            if (user is null)
            {
                var newUser = new User()
                {
                    Id = discordUser.Id,
                    Name = discordUser.Username,
                    FaceitPlayerId = faceitPlayer.player_id,
                    SteamID64 = null
                };
                await context.Users.AddAsync(newUser);
            }
            else
            {
                // TODO: should user be allowed to update? --> for now let's allow it
                user.FaceitPlayerId = faceitPlayer.player_id;
                context.Users.Update(user);
            }
            wasFound = await context.SaveChangesAsync() > 0;
        }
        return wasFound;
    }

    public async Task<FaceitPlayer> GetPlayerInfo(Guid? faceitPlayerId)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("FaceitApiKey"));
        var response = await _httpClient.GetAsync($"https://open.faceit.com/data/v4/players/{faceitPlayerId}");

        var faceitPlayer = JsonConvert.DeserializeObject<FaceitPlayer>(await response.Content.ReadAsStringAsync());

        if (faceitPlayer is null)
        {
            throw new FaceitServiceException($"Deserializing Faceit Player response from FaceIt API resulted in null");
        }

        return faceitPlayer;
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