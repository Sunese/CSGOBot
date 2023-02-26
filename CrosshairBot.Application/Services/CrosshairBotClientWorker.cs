//using CrosshairBot.Domain.SlashCommands;
//using CrosshairBot.Domain.SlashCommands.Handlers;
//using Discord;
//using Discord.Net;
//using Discord.WebSocket;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using Microsoft.VisualBasic.CompilerServices;
//using Newtonsoft.Json;

//namespace CrosshairBot.Application.Services
//{
//    public class CrosshairBotClientWorker : BackgroundService
//    {
//        private readonly ILogger<CrosshairBotClientWorker> logger;
//        private readonly DiscordSocketClient client;
//        private readonly ISlashCommands commands;

//        public CrosshairBotClientWorker(ILogger<CrosshairBotClientWorker> logger, DiscordSocketClient client, ISlashCommands commands)
//        {
//            this.logger = logger;
//            this.client = client;
//            this.commands = commands;
//        }

//        public override async Task StartAsync(CancellationToken stoppingToken)
//        {

//            try
//            {
//                logger.LogInformation("CrosshairBotClientWorker service started!");

//                //// TODO handle this properly s.t. it can be run from any directory and both windows and mac
//                //// /users/sune/git/crosshairbot/CrosshairBot
//                //string workingDirectory = Environment.CurrentDirectory;
//                //// secrets directory is one level up
//                //string secretsDirectory = Path.GetFullPath(Path.Combine(workingDirectory, @"../Secrets"));
//                //string tokenFile = Path.Combine(secretsDirectory, "token.txt");
//                //string token = File.ReadAllText(tokenFile);


//                var token = Environment.GetEnvironmentVariable("DiscordBotToken");

//                await client.LoginAsync(TokenType.Bot, token);
//                await client.StartAsync();

//                client.Ready += async () =>
//                {
//                    logger.LogInformation("Bot is connected");


//                    // Sample commands - should be moved somewhere else

//                    var list = new List<SlashCommandBuilder>();

//                    var helloCommand = new SlashCommandBuilder();
//                    helloCommand.WithName("hello");
//                    helloCommand.WithDescription("This is my first slash command!");
//                    list.Add(helloCommand);
                    
//                    var xhairofTheWeekCommand = new SlashCommandBuilder();
//                    xhairofTheWeekCommand.WithName("crosshairoftheweek");
//                    xhairofTheWeekCommand.WithDescription("The crosshair of current week's top player");
//                    list.Add(xhairofTheWeekCommand);
                    

//                    await commands.SetSlashCommands(list);
//                    client.SlashCommandExecuted += commands.Handle;
//                };

//            }
//            catch (ApplicationCommandException exception)
//            {
//                // If our command was invalid, we should catch an ApplicationCommandException. This exception contains the path of the error as well as the error message. You can serialize the Error field in the exception to get a visual of where your error is.
//                var json = JsonConvert.SerializeObject(exception.Errors, Formatting.Indented);

//                // You can send this error somewhere or just print it to the console, for this example we're just going to print it.
//                Console.WriteLine(json);
//            }
//        }

//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (!stoppingToken.IsCancellationRequested)
//            {
//                // TODO: do we need a worker queue to be consumed?
//                // TODO: do we need a background service/ worker at all?
//                //       I don't think so, since we are relying on events
//                logger.LogInformation("CrosshairBotClientWorker service executing!");
//                await Task.Delay(1000);
//            }
//        }
//    }
//}