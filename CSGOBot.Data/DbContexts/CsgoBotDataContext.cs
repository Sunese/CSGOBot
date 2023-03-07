using System.ComponentModel.DataAnnotations;
using CSGOBot.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace Repository.DbContexts;

public class CsgoBotDataContext : DbContext
{
    public DbSet<User> Users { get; set; }
    //public DbSet<FaceitPlayer> FaceitPlayers { get; set; }

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

    //protected override void OnModelCreating(ModelBuilder builder)
    //{
    //    //builder.Entity<FaceitPlayer>()
    //    //    .Property(e => e.friends_ids)
    //    //    .HasConversion(
    //    //        v => string.Join(',', v),
    //    //        v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
    //    builder.Entity<FaceitPlayer>()
    //        .Property(e => e.memberships)
    //        .HasConversion(
    //            v => string.Join(',', v),
    //            v => v.Split(',', StringSplitOptions.RemoveEmptyEntries));
    //    builder.Entity<Csgo>(b =>
    //    {
    //        b.HasKey(e => e.Id);
    //        b.Property(e => e.Id).ValueGeneratedOnAdd();
    //    });

    //    builder.Entity<Games>(b =>
    //    {
    //        b.HasKey(e => e.Id);
    //        b.Property(e => e.Id).ValueGeneratedOnAdd();
    //    });

    //}
}