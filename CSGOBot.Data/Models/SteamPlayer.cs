using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSGOBot.Data.Models;

public class SteamApiResponse
{
    public Response response { get; set; }
}

public class Response
{
    public Player[] players { get; set; }
}

public class Player
{
    public string steamid { get; set; }
    public int communityvisibilitystate { get; set; }
    public int profilestate { get; set; }
    public string personaname { get; set; }
    public int commentpermission { get; set; }
    public string profileurl { get; set; }
    public string avatar { get; set; }
    public string avatarmedium { get; set; }
    public string avatarfull { get; set; }
    public string avatarhash { get; set; }
    public int lastlogoff { get; set; }
    public int personastate { get; set; }
    public string realname { get; set; }
    public string primaryclanid { get; set; }
    public int timecreated { get; set; }
    public int personastateflags { get; set; }
    public string loccountrycode { get; set; }
}
