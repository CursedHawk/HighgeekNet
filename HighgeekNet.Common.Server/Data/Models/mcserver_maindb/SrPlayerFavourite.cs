using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class SrPlayerFavourite
{
    public string Uuid { get; set; } = null!;

    public long Timestamp { get; set; }

    public string SkinIdentifier { get; set; } = null!;

    public string? SkinVariant { get; set; }

    public string SkinType { get; set; } = null!;
}
