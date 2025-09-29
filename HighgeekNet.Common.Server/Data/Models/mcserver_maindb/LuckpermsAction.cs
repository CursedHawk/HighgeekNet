using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class LuckpermsAction
{
    public int Id { get; set; }

    public long Time { get; set; }

    public string ActorUuid { get; set; } = null!;

    public string ActorName { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string ActedUuid { get; set; } = null!;

    public string ActedName { get; set; } = null!;

    public string Action { get; set; } = null!;
}
