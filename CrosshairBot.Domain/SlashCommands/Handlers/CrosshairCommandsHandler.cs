using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;

namespace CrosshairBot.Domain.SlashCommands.Handlers;

public static class CrosshairCommandsHandler
{

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

    private static async Task<List<PlayerStatistics>> CallApi(string fullUrl)
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

    public static async Task Respond(SocketSlashCommand command)
    {

        var today = DateTime.Now;
        var aWeekAgo = today.AddDays(-7);

        var todayString = today.ToString("yyyy-MM-dd");
        var aWeekAgoString = aWeekAgo.ToString("yyyy-MM-dd");

        var top = "10";

        var url = @$"http://localhost:3000/players/{aWeekAgoString}/{todayString}";

        var statistics = await CallApi(url);

        var embedBuilder = new EmbedBuilder()
            .WithAuthor(command.User)
            .WithTitle("Crosshair of the week ⚡")
            .WithDescription($"Crosshair of the week is from {statistics[0].Nickname}")
            .WithColor(Color.Green)
            .WithCurrentTimestamp();

        await command.RespondAsync(embed: embedBuilder.Build());
    }

}
