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
    private readonly FaceitService _faceit;
    //private readonly HltvApiService _hltvApiService;
    //private readonly ProSettingsScraperService _proSettingsScraperService;
        

    // Constructor injection is also a valid way to access the dependencies
    public FaceitInteractionModule(
        InteractionHandler handler,
        FaceitService faceit)
    {
        _handler = handler;
        _faceit = faceit;
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

    [SlashCommand("faceitinfo", "Get Faceit info")]
    public async Task FaceitPlayerInfo()
        // todo: add choice for every user in guild that has registered (and 'myself')
        // maybe a bad idea? i believe arguments for slash commands are limited to like 20-30
        // maybe allow command user to mention a user that they want commands for
    {
        //_faceit.GetPlayerInfo(guid);
    }

}