using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOBot.Data.Models;

public class User
{
    [Key] // Attribute indicating that this is the primary key
    public ulong Id { get; set; }
    public string Name { get; set; }
    public string? SteamID64 { get; set; }
    public Guid? FaceitPlayerId { get; set; }
    public double BettingBalance { get; set; }
}