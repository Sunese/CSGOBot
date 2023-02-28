using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;

namespace CrosshairBot.SlashCommands.Util
{
    public static class CrosshairCommandUtil
    {
        public static async Task<List<PlayerStatistics>> CallHltvApi(string fullUrl)
        {
            HttpClient _client = new();
            var response = await _client.GetAsync(fullUrl);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("");
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            var stats = JsonConvert.DeserializeObject<List<PlayerStatistics>>(responseBody);
            if (stats == null)
            {
                throw new Exception("Deserialization into PlayerStatisticsList gave null");
            }
            return stats;
        }

        public static async Task<Crosshair> ScrapeSettings(string nickname)
        {
            HttpClient _client = new();
            var prosettingsUrl = $"https://prosettings.net/players/{nickname}";

            // First send a GET to see if ProSettings.net has a page for top player of the week
            if (!(await _client.GetAsync(prosettingsUrl)).IsSuccessStatusCode)
            {
                throw new PlayerNotFoundException();
            }

            var web = new HtmlWeb();
            var doc = web.Load(prosettingsUrl);

            // get the table with class name "settings"
            var settingsTables = doc.DocumentNode.SelectNodes("//table[@class='settings']");

            var crosshair = new Crosshair();
            var settings = new Dictionary<string, string>(500);

            foreach (var item in doc.DocumentNode.SelectNodes("//*[@data-field]"))
            {
                var attribute = item.GetAttributeValue("data-field", "default");
                var value = item.SelectSingleNode("td").InnerText;
                settings.TryAdd(attribute, value);
            }

            crosshair.cl_crosshair_drawoutline = settings["cl_crosshair_drawoutline"];
            crosshair.cl_crosshair_sniper_width = settings["cl_crosshair_sniper_width"];
            crosshair.cl_crosshairalpha = settings["cl_crosshairalpha"];
            crosshair.cl_crosshaircolor = settings["cl_crosshaircolor"];
            crosshair.cl_crosshaircolor_b = settings["cl_crosshaircolor_b"];
            crosshair.cl_crosshaircolor_g = settings["cl_crosshaircolor_g"];
            crosshair.cl_crosshaircolor_r = settings["cl_crosshaircolor_r"];
            crosshair.cl_crosshairdot = settings["cl_crosshairdot"];
            crosshair.cl_crosshairgap = settings["cl_crosshairgap"];
            crosshair.cl_crosshairsize = settings["cl_crosshairsize"];
            crosshair.cl_crosshairstyle = settings["cl_crosshairstyle"];
            crosshair.cl_crosshairthickness = settings["cl_crosshairthickness"];

            return crosshair;
        }


        public static string GenerateCrosshairString(Crosshair x)
        {
            string res = "";
            res = res + "cl_crosshair_drawoutline " + x.cl_crosshair_drawoutline + ";"
                  + "cl_crosshair_sniper_width " + x.cl_crosshair_sniper_width + ";"
                  + "cl_crosshairalpha " + x.cl_crosshairalpha + ";"
                  + "cl_crosshaircolor " + x.cl_crosshaircolor + ";"
                  + "cl_crosshaircolor_b " + x.cl_crosshaircolor_b + ";"
                  + "cl_crosshaircolor_g " + x.cl_crosshaircolor_g + ";"
                  + "cl_crosshaircolor_r " + x.cl_crosshaircolor_r + ";"
                  + "cl_crosshairdot " + x.cl_crosshairdot + ";"
                  + "cl_crosshairgap " + x.cl_crosshairgap + ";"
                  + "cl_crosshairsize " + x.cl_crosshairsize + ";"
                  + "cl_crosshairstyle " + x.cl_crosshairstyle + ";"
                  + "cl_crosshairthickness " + x.cl_crosshairthickness + ";";
            return res;
        }

        public class Settings
        {
            //public Mouse Mouse;
            public Crosshair Crosshair;
            //public Viewmodel Viewmodel;
            //public Bob Bob;
            //public LaunchOptions LaunchOptions;
            //public File Config;
            //public VideoSettings VideoeSettings;
            //public MonitorSettings MonitorSettings;
            //public int DigitalVibrance;
        }

        public class Crosshair
        {
            public string cl_crosshair_drawoutline { get; set; }
            public string cl_crosshairalpha { get; set; }
            public string cl_crosshaircolor { get; set; }
            public string cl_crosshaircolor_b { get; set; }
            public string cl_crosshaircolor_g { get; set; }
            public string cl_crosshaircolor_r { get; set; }
            public string cl_crosshairdot { get; set; }
            public string cl_crosshairgap { get; set; }
            public string cl_crosshairsize { get; set; }
            public string cl_crosshairstyle { get; set; }
            public string cl_crosshairthickness { get; set; }
            public string cl_crosshair_sniper_width { get; set; }
        }

        public class PlayerStatistics
        {
            public string Id;
            public string Team;
            public string Nickname;
            public string Slug;
            public string MapsPlayed;
            public string Kd;
            public string Rating;
        }

        public class PlayerNotFoundException : Exception
        {
            public PlayerNotFoundException()
            {
            }

            public PlayerNotFoundException(string message)
                : base(message)
            {
            }

            public PlayerNotFoundException(string message, Exception inner)
                : base(message, inner)
            {
            }
        }
    }
}
