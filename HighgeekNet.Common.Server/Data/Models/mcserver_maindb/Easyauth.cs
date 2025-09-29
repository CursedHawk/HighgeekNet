using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class Easyauth
{
    public int Id { get; set; }

    public string Uuid { get; set; } = null!;

    public string Data { get; set; } = null!;
}
