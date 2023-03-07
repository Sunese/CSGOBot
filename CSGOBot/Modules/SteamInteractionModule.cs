using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;

namespace CSGOBot.Modules
{
    public class SteamInteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        // Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
        public InteractionService Commands { get; set; }

        private readonly InteractionHandler _handler;
        //private readonly FaceitService _faceit;
        //private readonly HltvApiService _hltvApiService;
        //private readonly ProSettingsScraperService _proSettingsScraperService;


        // Constructor injection is also a valid way to access the dependencies
        public SteamInteractionModule(
            InteractionHandler handler)
            //FaceitService faceit)
        {
            _handler = handler;
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
    }
}
