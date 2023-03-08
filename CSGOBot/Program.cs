using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Discord.Commands;
using InteractionFramework;
using Serilog;
using Serilog.AspNetCore;
using CSGOBot.Services;
using Serilog.Events;

namespace CSGOBot;
public class Program
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _services;

    private readonly DiscordSocketConfig _socketConfig = new()
    {
        GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.MessageContent,
        AlwaysDownloadUsers = true,
    };

    public Program()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .WriteTo.File("logs/CSGOBotLog.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        _configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables(prefix: "DC_")
            .AddJsonFile("appsettings.json", optional: true)
            .Build();

        _services = new ServiceCollection()
            .AddSingleton(_configuration)
            .AddSingleton(_socketConfig)
            .AddSingleton<CommandService>()
            .AddScoped<HltvApiService>()
            .AddScoped<ProSettingsScraperService>()
            .AddScoped<SteamService>()
            .AddScoped<FaceitService>()
            .AddSingleton<DiscordSocketClient>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddSingleton<InteractionHandler>()
            .AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true))
            .BuildServiceProvider();
    }

    static void Main(string[] args)
        => new Program().RunAsync()
            .GetAwaiter()
            .GetResult();

    public async Task RunAsync()
    {
        var client = _services.GetRequiredService<DiscordSocketClient>();

        client.Log += LogAsync;

        // Here we can initialize the service that will register and execute our commands
        await _services.GetRequiredService<InteractionHandler>()
            .InitializeAsync();

        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();
        var secret = config["DiscordBotToken"];
        var connectionString = config["CsgoBotDataConnectionString"];
        var faceitApiKey = config["FaceitApiKey"];
        var steamApiKey = config["SteamApiKey"];
        Environment.SetEnvironmentVariable("DiscordBotToken", secret);
        Environment.SetEnvironmentVariable("CsgoBotDataConnectionString", connectionString);
        Environment.SetEnvironmentVariable("FaceitApiKey", faceitApiKey);
        Environment.SetEnvironmentVariable("SteamApiKey", steamApiKey);


        // Bot token can be provided from the Configuration object we set up earlier
        await client.LoginAsync(TokenType.Bot, secret);
        await client.StartAsync();

        // Never quit the program until manually forced to.
        await Task.Delay(Timeout.Infinite);
    }

    private static Task LogAsync(LogMessage message)
    {
        var severity = message.Severity switch
        {
            LogSeverity.Critical => LogEventLevel.Fatal,
            LogSeverity.Error => LogEventLevel.Error,
            LogSeverity.Warning => LogEventLevel.Warning,
            LogSeverity.Info => LogEventLevel.Information,
            LogSeverity.Verbose => LogEventLevel.Verbose,
            LogSeverity.Debug => LogEventLevel.Debug,
            _ => LogEventLevel.Information
        };

        Log.Write(severity, message.Exception, "[{Source}] {Message}", message.Source, message.Message);

        return Task.CompletedTask;
    }

    public static bool IsDebug()
    {
#if DEBUG
        return true;
#else
        return false;
#endif
    }
}
