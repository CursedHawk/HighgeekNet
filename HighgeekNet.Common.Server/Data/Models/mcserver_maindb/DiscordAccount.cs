using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class DiscordAccount
{
    public int Link { get; set; }

    public string Discord { get; set; } = null!;

    public string Playername { get; set; } = null!;

    public string Uuid { get; set; } = null!;
}
