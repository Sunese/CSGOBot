using Microsoft.Extensions.Hosting;
using Serilog;

var builder = WebApplication.CreateApplicationBuilder(args)
    .Build();


builder.Run();