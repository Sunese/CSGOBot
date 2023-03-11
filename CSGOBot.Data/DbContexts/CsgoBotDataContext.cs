using System.ComponentModel.DataAnnotations;
using CSGOBot.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace Repository.DbContexts;

public class CsgoBotDataContext : DbContext
{
    public DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#if DEBUG
        var config = new ConfigurationBuilder()
            .AddUserSecrets<Program>()
            .Build();

        var connectionString = config["CsgoBotDataConnectionString"];
        Environment.SetEnvironmentVariable("CsgoBotDataConnectionString", connectionString);
#endif
#if (!DEBUG)
            var connectionString = Environment.GetEnvironmentVariable("CsgoBotDataConnectionString");
#endif
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}