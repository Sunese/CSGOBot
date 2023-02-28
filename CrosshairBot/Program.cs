using System.Net.NetworkInformation;
using System.Reflection;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Events;
using AutoMapper;

namespace CrosshairBot;

public class Bot
{
    private static ServiceProvider ConfigureServices()
    {
        return new ServiceCollection()
            .AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Bot>())
            .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig
            {
                AlwaysDownloadUsers = true,
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.AllUnprivileged,
                LogLevel = LogSeverity.Info
            }))
            .AddSingleton<DiscordEventListener>()
            .AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()))
            .AddAutoMapper(typeof(Bot))
            .AddLogging(builder => builder.AddSerilog())
            .BuildServiceProvider();
    }

    public static async Task Main()
    {
        await new Bot().RunAsync();
    }

    private async Task RunAsync()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        var config = new ConfigurationBuilder()
            .AddUserSecrets<Bot>()
            .Build();
        var secret = config["DiscordBotToken"];

        await using var services = ConfigureServices();

        var client = services.GetRequiredService<DiscordSocketClient>();
        client.Log += LogAsync;

        var listener = services.GetRequiredService<DiscordEventListener>();
        await listener.StartAsync();

        await client.LoginAsync(TokenType.Bot, secret);
        await client.StartAsync();

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
}