using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class SrCustomSkin
{
    public string Name { get; set; } = null!;

    public string Value { get; set; } = null!;

    public string Signature { get; set; } = null!;

    public string? DisplayName { get; set; }
}
