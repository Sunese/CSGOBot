using System;
using System.Net.Http;
using System.Threading.Tasks;
using CSGOBot.Enums;
using CSGOBot.Services;
using Discord;
using Discord.Interactions;
using InteractionFramework;

namespace CSGOBot.Modules;

public class CrosshairInteractionModule : InteractionModuleBase<SocketInteractionContext>
{
    private readonly HltvApiService _hltvApiService;
    private readonly ProSettingsScraperService _proSettingsScraperService;
    
    public CrosshairInteractionModule(
        HltvApiService hltvApiService, 
        ProSettingsScraperService proSettingsScraperService)
    {
        _hltvApiService = hltvApiService;
        _proSettingsScraperService = proSettingsScraperService;
    }
    
    [SlashCommand("procrosshair", "Get a Pro CS:GO Player Crosshair")]
    public async Task ProCrosshair([Summary(description: "Top rated HLTV player of the ...")] CrosshairOfThe time)
    {
        await DeferAsync();
        var message = time == CrosshairOfThe.AllTime ? "You picked top rated HLTV player of all time." : $"You picked top rated HLTV player of the {time.ToString().ToLower()}.";
        await ModifyOriginalResponseAsync(x => x.Content = $"{message} Figuring out who that is... <a:HACKERMANS:587667462670123060>");
        try
        {
            var topPlayers = await _hltvApiService.GetTopHltvPlayers(time);
            var topPlayer = topPlayers[0].Nickname;

            message = time == CrosshairOfThe.AllTime 
                ? $"Top rated player of all time is {topPlayer} :boom:" 
                : $"Top rated player of the {time.ToString().ToLower()} is {topPlayer} :boom:";

            await ModifyOriginalResponseAsync(x => x.Content = message);
            // This leads to nice smooth, transitional experience
            await Task.Delay(1500);

            await ModifyOriginalResponseAsync(x => x.Content = $"Looking for {topPlayer}'s crosshair :mag:");
            // This leads to nice smooth, transitional experience
            await Task.Delay(1500);

            var crosshair = await _proSettingsScraperService.Scrape(topPlayer);
            var embed = new EmbedBuilder()
                .WithTitle(@$"{topPlayer} crosshair :gun:")
                .WithDescription(crosshair)
                .Build();
            await ModifyOriginalResponseAsync((x) =>
            {
                x.Embed = embed;
                x.Content = @$"Top player of the {time.ToString().ToLower()} is {topPlayer} :boom:";
            });
        }
        catch (HttpRequestException e)
        {
            await FollowupAsync("Could not reach HLTV API :sob:");
        }
        catch (ProSettingsScraperService.PlayerNotFoundException e)
        {
            await FollowupAsync($"{e.Message} :sob:");
        }
        catch (Exception e)
        {
            await FollowupAsync("Something went wrong with the HLTV API :sob:");
        }
    }
}