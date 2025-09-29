using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class SrCache
{
    public string Name { get; set; } = null!;

    public string? Uuid { get; set; }

    public long Timestamp { get; set; }
}
