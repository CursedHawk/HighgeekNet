using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class LuckpermsPlayer
{
    public string Uuid { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string PrimaryGroup { get; set; } = null!;
}
