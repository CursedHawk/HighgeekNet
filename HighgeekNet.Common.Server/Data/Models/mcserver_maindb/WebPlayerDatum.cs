using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class WebPlayerDatum
{
    public string? Uuid { get; set; }

    public string Name { get; set; } = null!;

    public string? Plandata { get; set; }
}
