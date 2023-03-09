using CSGOBot.Data.Models;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;
using Repository.DbContexts;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CSGOBot.Modules
{
    public class RegisterInteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        // Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        public InteractionService Commands { get; set; }

        private readonly InteractionHandler _handler;
        private readonly SteamService _steam;
        private readonly FaceitService _faceit;
        //private readonly HltvApiService _hltvApiService;
        //private readonly ProSettingsScraperService _proSettingsScraperService;


        // Constructor injection is also a valid way to access the dependencies
        public RegisterInteractionModule(
            InteractionHandler handler,
            SteamService steam,
            FaceitService faceit)
        {
            _handler = handler;
            _steam = steam;
            _faceit = faceit;
            //_faceit = faceit;
            //_hltvApiService = hltvApiService;
            //_proSettingsScraperService = proSettingsScraperService;
        }

        [Group("register", "Register stuff to your Discord profile")]
        public class RegisterGroup : InteractionModuleBase<SocketInteractionContext>
        {
            [SlashCommand("steam", "Connect your Steam account to your Discord profile!")]
            public async Task RegisterSteam() => 
                await Context.Interaction.RespondWithModalAsync<RegisterSteamModal>("register_steam");

            [SlashCommand("faceit", "Connect your Faceit account to your Discord profile!")]
            public async Task RegisterFaceit() =>
                await Context.Interaction.RespondWithModalAsync<RegisterFaceitModal>("register_faceit");
        }

        // Responds to the modal.
        [ModalInteraction("register_steam")]
        public async Task SteamModalResponse(RegisterSteamModal modal)
        {
            await DeferAsync(ephemeral: true);
            await FollowupAsync("Validating SteamID64 and linking to your Discord profile... :mag:", ephemeral: true);

            var modal_steamId64 = modal.SteamID64;

            if (modal_steamId64.Length is > 20 or < 15 ||
                !ulong.TryParse(modal_steamId64, out _))
            {
                var embed = new EmbedBuilder()
                    .WithTitle("steamid.io")
                    .WithUrl("https://steamid.io/")
                    .WithDescription(
                        "It does not look like you provided me with a SteamID64 value :warning: You can look up your SteamID64 here: https://steamid.io/")
                    .Build();
                await FollowupAsync(embed: embed, ephemeral: true);
            }
            else
            {
                try
                {
                    var wasRegistered = await _steam.TryRegisterUserAsync(Context.User, modal_steamId64);
                    if (wasRegistered)
                    {
                        var embed = new EmbedBuilder()
                            .WithTitle("Success!")
                            .WithDescription($"Steam ID: {modal_steamId64} has been linked to your discord account! :heart:\n"+
                                             "Want to see someone's Steam profile?\n Right click their Discord name :arrow_right: apps :arrow_right: steaminfo")
                            //.WithAuthor(modal.User)
                            .Build();
                        await FollowupAsync(embed: embed, ephemeral: true);
                    }
                    else
                    {
                        var embed = new EmbedBuilder()
                            .WithTitle("Something went wrong :sadge:")
                            .WithDescription($"Steam ID: {modal_steamId64} has not been linked to your discord account :warning:")
                            //.WithAuthor(modal.User)
                            .Build();
                        await FollowupAsync(embed: embed, ephemeral: true);
                    }
                }
                catch (SteamService.SteamServiceException e)
                {
                    var embed = new EmbedBuilder()
                        .WithTitle("Error :warning:")
                        .WithDescription($"Steam ID: {modal_steamId64} has not been linked :sadge:\nError occurred with Steam API: {e.Message}")
                        .Build();
                    await FollowupAsync(embed: embed, ephemeral: true);
                }
            }
        }

        // Responds to the modal.
        [ModalInteraction("register_faceit")]
        public async Task FaceitModalResponse(RegisterFaceitModal modal)
        {
            await DeferAsync(ephemeral: true);
            await FollowupAsync("Locating Faceit user and linking to your Discord profile... :mag:", ephemeral: true);
            var modal_faceitUsername = modal.faceitUsername;

            var registered = await _faceit.TryRegisterUserAsync(Context.User, modal_faceitUsername);
            if (registered)
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Success!")
                    .WithDescription($"Faceit user {modal_faceitUsername} has been linked to your discord account! :heart:\n" +
                                     "Want to see someone's Faceit profile?\n Right click their Discord name :arrow_right: apps :arrow_right: faceitinfo")
                    .WithAuthor(Context.User)
                    .Build();
                await FollowupAsync(embed: embed, ephemeral: true);
            }
            else
            {
                var embed = new EmbedBuilder()
                    .WithTitle("Something went wrong :sadge:")
                    .WithDescription($"Faceit account: {modal_faceitUsername} has not been linked to your discord account :warning:")
                    .WithAuthor(Context.User)
                    .Build();
                await FollowupAsync(embed: embed, ephemeral: true);
            }
        }

        public class RegisterSteamModal : IModal
        {
            public string Title => "Registering Steam to your profie";
            // Strings with the ModalTextInput attribute will automatically become components.
            [InputLabel("What is your steamID64?")]
            [ModalTextInput("steam_username", placeholder: "SteamID64 (you can find it on steamid.io)", maxLength: 30)]
            public string SteamID64 { get; set; }
        }

        public class RegisterFaceitModal : IModal
        {
            public string Title => "Registering Faceit to your profie";
            // Strings with the ModalTextInput attribute will automatically become components.
            [InputLabel("What is your faceit username?")]
            [ModalTextInput("faceit_username", placeholder: "NOTE: CASE SENSITIVE", maxLength: 30)]
            public string faceitUsername { get; set; }
        }
    }
}
