using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;
using System.Threading.Tasks;

namespace CSGOBot.Modules
{
    public class RegisterInteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly SteamService _steam;
        private readonly FaceitService _faceit;
        
        public RegisterInteractionModule(
            SteamService steam,
            FaceitService faceit)
        {
            _steam = steam;
            _faceit = faceit;
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
