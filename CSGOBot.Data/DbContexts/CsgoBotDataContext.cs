using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Repository.DbContexts
{
    public class CsgoBotDataContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        //public DbSet<User> Users { get; set; } <-- More tables can be added

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

    public class User
    {
        [Key] // Attribute indicating that this is the primary key
        public ulong Id { get; set; }
        public string Name { get; set; }
        public string? SteamID64 { get; set; }
        public string? FaceitPlayerId { get; set; }
    }
}
