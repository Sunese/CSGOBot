using CSGOBot.Data.Models;
using Discord.WebSocket;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Repository.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Microsoft.AspNetCore.Mvc.TagHelpers.Cache;

namespace CSGOBot.Services;

public class UserService
{
    private readonly ILogger<UserService> _logger;

    public UserService(ILogger<UserService> logger)
    {
        _logger = logger;
    }

    public async Task<bool> TryRegisterUserAsync(SocketUser discordUser)
    {
        await using var context = new CsgoBotDataContext();
        if (!await ExistsAsync(discordUser.Id))
        {
            
            var newUser = new User()
            {
                Id = discordUser.Id,
                Name = discordUser.Username,
                FaceitPlayerId = null,
                SteamID64 = null,
                BettingBalance = 100.0 // set starting betting balance to 100
            };
            await context.Users.AddAsync(newUser);
        }
        else // user already exists, do nothing
        {
            return false;
        }
        return await context.SaveChangesAsync() > 0;
    }

    public async Task<bool> ExistsAsync(ulong discordUserId)
    {
        await using var context = new CsgoBotDataContext();
        var user = await context.Users.FindAsync(discordUserId);
        return user != null;
    }

    public class UserServiceException : Exception
    {
        public UserServiceException()
        {
        }

        public UserServiceException(string message)
            : base(message)
        {
        }

        public UserServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }


}