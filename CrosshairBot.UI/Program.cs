using System.Reflection;
using CrosshairBot.Domain;
using CrosshairBot.Domain.SlashCommands;
using Discord.WebSocket;
using Serilog;
using CrosshairBot.Domain.SlashCommands.Handlers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using System;
using CrosshairBot.Core.SlashCommands;
using CrosshairBot.UI;
using CrosshairBot.Application;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var secret = config["DiscordBotToken"];

Environment.SetEnvironmentVariable("DiscordBotToken", secret);

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<DiscordSocketClient>(); // <-- can be manually instantiated to pass configs
        //services.AddSingleton<ISlashCommands, SlashCommand>();
        //services.AddTransient<IExampleTransientService, ExampleTransientService>();
        //services.AddScoped<IExampleScopedService, ExampleScopedService>();
        //services.AddSingleton<IExampleSingletonService, ExampleSingletonService>();
        //services.AddTransient<ServiceLifetimeReporter>();
        //services.AddSingleton<IConfiguration>(provider => configuration);
        //services.AddSingleton<ICrosshairCommandsHandler, CrosshairCommandsHandler>();
    })
    .UseSerilog()
    .Build();

await CrosshairBotInitializer.Initialize(
    host.Services.GetService<DiscordSocketClient>(),
    host.Services.GetService<ILogger<CrosshairBotInitializer>>(),
    host.Services.GetService<ILogger<SlashCommandHandler>>());



await host.RunAsync();