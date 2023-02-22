using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;

namespace CrosshairBot.Domain.SlashCommands.Handlers;

public class CrosshairCommandsHandler : ICrosshairCommandsHandler
{
    private ILogger<ICrosshairCommandsHandler> logger;
    public CrosshairCommandsHandler(ILogger<CrosshairCommandsHandler> logger)
    {
        this.logger = logger;
    }
    public class Crosshair {
        public string cl_crosshair_drawoutline {get; set;}
        public string cl_crosshairalpha {get; set;}
        public string cl_crosshaircolor {get; set;}
        public string cl_crosshaircolor_b {get; set;}
        public string cl_crosshaircolor_g {get; set;}
        public string cl_crosshaircolor_r {get; set;}
        public string cl_crosshairdot {get; set;}
        public string cl_crosshairgap {get; set;}
        public string cl_crosshairsize {get; set;}
        public string cl_crosshairstyle {get; set;}
        public string cl_crosshairthickness {get; set;}
        public string cl_crosshair_sniper_width {get; set;}
    }

    private class PlayerStatistics
    {
        public string Id;
        public string Team;
        public string Nickname;
        public string Slug;
        public string MapsPlayed;
        public string Kd;
        public string Rating;
    }

    private async Task<List<PlayerStatistics>> CallHltvApi(string fullUrl)
    {
        HttpClient client = new HttpClient();
        var response = await client.GetAsync(fullUrl);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();
        var stats = JsonConvert.DeserializeObject<List<PlayerStatistics>>(responseBody);
        if (stats == null)
        {
            throw new Exception("Deserialization into PlayerStatisticsList gave null");
        }
        return stats;
    }

    private async Task<Crosshair> ScrapeSettings(string nickname) 
    {
        // TODO: scrape settings info from the website
        var prosettingsUrl = $"https://prosettings.net/players/{nickname}";
        var web = new HtmlWeb();
        var doc = web.Load(prosettingsUrl);

        // get the table with class name "settings"
        var settingsTable = doc.DocumentNode.SelectSingleNode("//table[@class='settings']");

        foreach (HtmlNode row in settingsTable.SelectNodes("tr"))
        {
            foreach (HtmlNode cell in row.SelectNodes("th|td"))
            {
                logger.LogInformation(cell.InnerText);
            }
        }
        
        

        // instantiate a Crosshair object and populate values 
        // each class name has the corresponding field name from the class
        var crosshair = new Crosshair();

        return crosshair;
    }

    public async Task Respond(SocketSlashCommand command)
    {

        var today = DateTime.Now;
        var aWeekAgo = today.AddDays(-7);

        var todayString = today.ToString("yyyy-MM-dd");
        var aWeekAgoString = aWeekAgo.ToString("yyyy-MM-dd");

        var top = "10";

        var url = @$"http://localhost:3000/players/{aWeekAgoString}/{todayString}";

        //var statistics = await CallHltvApi(url);

        //var topPlayerOfTheWeek = statistics[0].Nickname;

        logger.LogInformation("Inside Respond");

        var crosshairOfTopPlayer = await ScrapeSettings("s1mple");

        




        var embedBuilder = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Crosshair of the week ⚡")
            .WithDescription($"Scraped: {crosshairOfTopPlayer}")
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        await command.RespondAsync(embed: embedBuilder.Build());
    }

}
