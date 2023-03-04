using System;
using System.Net.Http;
using System.Threading.Tasks;
using CSGOBot.Enums;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;

namespace CSGOBot.Modules;

// Interaction modules must be public and inherit from an IInteractionModuleBase
public class FaceitInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    // Dependencies can be accessed through Property injection, public properties with public setters will be set by the service provider
    public InteractionService Commands { get; set; }

    private readonly InteractionHandler _handler;
    //private readonly HltvApiService _hltvApiService;
    //private readonly ProSettingsScraperService _proSettingsScraperService;
        

    // Constructor injection is also a valid way to access the dependencies
    public FaceitInteractionModule(
        InteractionHandler handler, 
        HltvApiService hltvApiService, 
        ProSettingsScraperService proSettingsScraperService)
    {
        _handler = handler;
        //_hltvApiService = hltvApiService;
        //_proSettingsScraperService = proSettingsScraperService;
    }

    // You can use a number of parameter types in you Slash Command handlers (string, int, double, bool, IUser, IChannel, IMentionable, IRole, Enums) by default. Optionally,
    // you can implement your own TypeConverters to support a wider range of parameter types. For more information, refer to the library documentation.
    // Optional method parameters(parameters with a default value) also will be displayed as optional on Discord.

    // [Summary] lets you customize the name and the description of a parameter
    [SlashCommand("registerfaceit", "Connect your Faceit account to your Discord profile!")]
    public async Task RegisterFaceit()
    {
        var modal = new ModalBuilder()
            .WithTitle("Registering Faceit to your account")
            .WithCustomId("register_faceit")
            .AddTextInput(
                label: "What is your faceit username?",
                customId: "faceit_username",
                placeholder: "Faceit username (case sensitive)");
        await RespondWithModalAsync(modal.Build());
    }
}