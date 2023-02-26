using System.Reflection;
using CrosshairBot.Application.Services;
using CrosshairBot.Domain;
using CrosshairBot.Domain.SlashCommands;
using Discord.WebSocket;
using MediatR;
using Serilog;
using CrosshairBot.Domain.SlashCommands.Handlers;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

IConfiguration config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var secret = config.GetValue<string>("DiscordBotToken");

Environment.SetEnvironmentVariable("DiscordBotToken", secret);

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<DiscordSocketClient>(); // <-- can be manually instantiated to pass configs
        //services.AddSingleton<ISlashCommands, SlashCommands>();
        //services.AddTransient<IExampleTransientService, ExampleTransientService>();
        //services.AddScoped<IExampleScopedService, ExampleScopedService>();
        //services.AddSingleton<IExampleSingletonService, ExampleSingletonService>();
        //services.AddTransient<ServiceLifetimeReporter>();
        //services.AddSingleton<IConfiguration>(provider => configuration);
        services.AddSingleton<ICrosshairCommandsHandler, CrosshairCommandsHandler>();
    })
    .UseSerilog()
    .Build();

await CrosshairBotInitializer.Initialize(
    host.Services.GetService<DiscordSocketClient>(),
    host.Services.GetService<ILogger<CrosshairBotInitializer>>());

await host.RunAsync();