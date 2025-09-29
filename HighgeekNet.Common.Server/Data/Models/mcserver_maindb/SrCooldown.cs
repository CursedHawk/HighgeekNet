using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class SrCooldown
{
    public string Uuid { get; set; } = null!;

    public string GroupName { get; set; } = null!;

    public long CreationTime { get; set; }

    public long Duration { get; set; }
}
