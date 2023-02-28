using CrosshairBot.NotificationHandlers;
using Discord;
using Discord.WebSocket;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using CrosshairBot.SlashCommands.Util;

namespace CrosshairBot.SlashCommands;

public class CrosshairOfTheWeekCommand
{
    public SlashCommandProperties SlashCommandProperties;

    public CrosshairOfTheWeekCommand()
    {
        var command = new SlashCommandBuilder();
        // IMPORTANT: Slash command names must be all lowercase!
        command.WithName("crosshairoftheweek");
        command.WithDescription("This will retrieve the top player of this week's crosshair!");
        SlashCommandProperties = command.Build();
    }

    public async Task<Action<MessageProperties>> MessageAction(SocketSlashCommand command)
    {
        var today = DateTime.Now;
        var aWeekAgo = today.AddDays(-7);

        var todayString = today.ToString("yyyy-MM-dd");
        var aWeekAgoString = aWeekAgo.ToString("yyyy-MM-dd");

        var top = "10";

        var url = @$"http://localhost:3000/players/{aWeekAgoString}/{todayString}";

        var embedBuilder = new EmbedBuilder();

        try
        {
            List<CrosshairCommandUtil.PlayerStatistics> statistics = 
                await CrosshairCommandUtil.CallHltvApi(url);
            var topPlayerOfTheWeek = statistics[0].Nickname;
            try
            {
                var crosshairOfTopPlayer = await CrosshairCommandUtil.ScrapeSettings(topPlayerOfTheWeek);
                embedBuilder
                    .WithAuthor(command.User)
                    .WithTitle($"Player of the week is {topPlayerOfTheWeek} ⚡")
                    .WithDescription(CrosshairCommandUtil.GenerateCrosshairString(crosshairOfTopPlayer))
                    .WithColor(Color.Green)
                    .WithCurrentTimestamp();
            }
            catch (CrosshairCommandUtil.PlayerNotFoundException)
            {
                embedBuilder
                    .WithAuthor(command.User)
                    .WithTitle($"Player of the week is {topPlayerOfTheWeek} ⚡")
                    .WithDescription($"No crosshair info was found for player :(")
                    .WithColor(Color.Red)
                    .WithCurrentTimestamp();
            }
        }
        catch (HttpRequestException e)
        {
            embedBuilder
                .WithTitle("Error occurred")
                .WithDescription("Could not reach HLTV API :(");
        }
        catch (Exception e)
        {
            embedBuilder
                .WithTitle("Error occurred")
                .WithDescription("Unknown error occurred :(");
        }

        return await Task.FromResult(
            new Action<MessageProperties>(
                x => x.Embed = embedBuilder.Build()));
    }
}
