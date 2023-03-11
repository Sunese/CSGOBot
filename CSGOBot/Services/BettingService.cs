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

public class BettingService
{
    private readonly ILogger<BettingService> _logger;
    private readonly UserService _user;

    public BettingService(ILogger<BettingService> logger, UserService user)
    {
        _logger = logger;
        _user = user;
    }

    // This assumes there has already been a check for whether user exists
    public async Task<double> GetBettingBalanceAsync(ulong discordUserId)
    {
        await using var context = new CsgoBotDataContext();
        var user = await context.Users.FindAsync(discordUserId);
        return user.BettingBalance;
    }

    public class BettingServiceException : Exception
    {
        public BettingServiceException()
        {
        }

        public BettingServiceException(string message)
            : base(message)
        {
        }
        
        public BettingServiceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }


}