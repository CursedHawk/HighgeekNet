using System;
using System.Collections.Generic;

namespace HighgeekNet.Common.Server.Data.Models.mcserver_maindb;

public partial class WebMinecraftuser
{
    public string Uuid { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string ApplicationUserName { get; set; } = null!;

    public string ApplicationUserId { get; set; } = null!;

    public string ApplicationUserEmail { get; set; } = null!;

    public string IsPremium { get; set; } = null!;

    public string? PremiumUuid { get; set; }

    public string? SkinTexture { get; set; }

    public string? SkinHeadTexture { get; set; }

    public string? LpUserGroup { get; set; }

    public byte[]? SkinHeadTextureImage { get; set; }

    public byte[]? EcoId { get; set; }
}
