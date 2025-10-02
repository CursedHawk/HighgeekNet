using HighgeekNet.Blazor.Client.Services.SignalR.Permissions;
using HighgeekNet.Common.Server.Services.Redis;
using Microsoft.AspNetCore.SignalR;
using MudBlazor;

namespace HighgeekNet.Blazor.Services.SignalR.Permissions
{

    public class PermissionsService
    {
        private readonly IHubContext<PermissionsHub, IPermissionClient> _permHub;

        public PermissionsService(IHubContext<PermissionsHub, IPermissionClient> permHub)
        {
            _permHub = permHub;
        }

    }
}
