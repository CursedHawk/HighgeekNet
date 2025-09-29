using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class SrPlayer
{
    public string Uuid { get; set; } = null!;

    public string? SkinIdentifier { get; set; }

    public string? SkinVariant { get; set; }

    public string? SkinType { get; set; }
}
