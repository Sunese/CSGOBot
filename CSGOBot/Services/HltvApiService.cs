using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CSGOBot.Enums;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSGOBot.Services;

public class HltvApiService
{
    private readonly ILogger<HltvApiService> _logger;
    private readonly HttpClient _httpClient = new HttpClient
    {
        Timeout = TimeSpan.FromSeconds(5)
    };

    public HltvApiService(ILogger<HltvApiService> logger)
    {
        _logger = logger;
    }

    public async Task<List<HltvPlayer>> GetTopHltvPlayers(CrosshairOfThe time)
    {
        List<HltvPlayer> response = new List<HltvPlayer>();
        string url = "http://localhost:3000/players";
        switch (time)
        {
            case CrosshairOfThe.AllTime:
                url = $"{url}";
                response = await Send(url);
                break;
            case CrosshairOfThe.Year:
                // Get the first day of the year
                var from = new DateTime(DateTime.Today.Year, 1, 1).ToString("yyyy-MM-dd");
                var to = DateTime.Today.ToString("yyyy-MM-dd");
                url = $"{url}/{from}/{to}";
                response = await Send(url);
                break;
            case CrosshairOfThe.Month:
                // Get the first day of the month
                from = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).ToString("yyyy-MM-dd");
                to = DateTime.Today.ToString("yyyy-MM-dd");
                url = $"{url}/{from}/{to}";
                response = await Send(url);
                break;
            case CrosshairOfThe.Week:
                // Get the first day of the week
                from = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).ToString("yyyy-MM-dd");
                to = DateTime.Today.ToString("yyyy-MM-dd");
                url = $"{url}/{from}/{to}";
                response = await Send(url);
                break;
            case CrosshairOfThe.Day:
                from = DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                to = DateTime.Today.ToString("yyyy-MM-dd");
                url = $"{url}/{from}/{to}";
                response = await Send(url);
                break;
            default:
                break;
        }
        return response;
    }

    public async Task<List<HltvPlayer>> Send(string url)
    {
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException("");
        }
        string responseBody = await response.Content.ReadAsStringAsync();
        var stats = JsonConvert.DeserializeObject<List<HltvPlayer>>(responseBody);
        if (stats == null || stats.Count < 1)
        {
            throw new Exception("Deserialization into HltvPlayerList gave null");
        }
        return stats;
    }

    public class HltvPlayer
    {
        public string Id;
        public string Team;
        public string Nickname;
        public string Slug;
        public string MapsPlayed;
        public string Kd;
        public string Rating;
    }
}