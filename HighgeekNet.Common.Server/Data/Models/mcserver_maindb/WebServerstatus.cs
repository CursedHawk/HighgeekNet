using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class WebServerstatus
{
    public string Name { get; set; } = null!;

    public string? Players { get; set; }

    public string? Maxplayers { get; set; }

    public string? Playerslist { get; set; }

    public string? Online { get; set; }

    public string Order { get; set; } = null!;
}
