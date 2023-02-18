using System.Reflection;
using CrosshairBot.Application.Services;
using CrosshairBot.Domain.SlashCommands;
using Discord.WebSocket;
using MediatR;
using Serilog;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();


using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<CrosshairBotClientWorker>();
        services.AddSingleton<DiscordSocketClient>();
        services.AddSingleton<ISlashCommands, SlashCommands>();
        //services.AddTransient<IExampleTransientService, ExampleTransientService>();
        //services.AddScoped<IExampleScopedService, ExampleScopedService>();
        //services.AddSingleton<IExampleSingletonService, ExampleSingletonService>();
        //services.AddTransient<ServiceLifetimeReporter>();
    })
    .UseSerilog()
    .Build();

await host.RunAsync();