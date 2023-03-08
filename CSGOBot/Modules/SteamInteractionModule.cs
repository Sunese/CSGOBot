using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGOBot.Data.Models;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;
using Repository.DbContexts;

namespace CSGOBot.Modules
{
    public class SteamInteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        // Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        public InteractionService Commands { get; set; }

        private readonly InteractionHandler _handler;
        private readonly SteamService _steam;
        //private readonly FaceitService _faceit;
        //private readonly HltvApiService _hltvApiService;
        //private readonly ProSettingsScraperService _proSettingsScraperService;


        // Constructor injection is also a valid way to access the dependencies
        public SteamInteractionModule(
            InteractionHandler handler,
            SteamService steam)
        {
            _handler = handler;
            _steam = steam;
            //_faceit = faceit;
            //_hltvApiService = hltvApiService;
            //_proSettingsScraperService = proSettingsScraperService;
        }

        [SlashCommand("registersteam", "Connect your Steam account to your Discord profile!")]
        public async Task RegisterSteam()
        {
            var modal = new ModalBuilder()
                .WithTitle("Registering Steam to your account")
                .WithCustomId("register_steam")
                .AddTextInput(
                    label: "What is your steamID64?",
                    customId: "steamid64",
                    placeholder: "SteamID64 (you can find it on steamid.io)");

            await RespondWithModalAsync(modal.Build());
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

            if (dbUser == null)
            {
                // user does not exist
                await FollowupAsync("User does not exist in database. They should execute the /registersteam or /registerfaceit command to register.");
                return;
            }

            if (dbUser is { SteamID64: null })
            {
                // user exists but faceit is not registered
                await FollowupAsync("User's Steam info does not exist in database. They should execute the /registersteam or /registerfaceit command to register.", ephemeral: true);
                return;
            }

            if (dbUser.SteamID64 != null)
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
                return;
            }
        }


    }
}
