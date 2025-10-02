using HighgeekNet.Blazor.Client.Services.SignalR.Permissions;
using HighgeekNet.Common.Server.Services;
using Microsoft.AspNetCore.SignalR;

namespace HighgeekNet.Blazor.Services.SignalR.Permissions
{
    public class PermissionsHub : Hub<IPermissionClient>, IPermissionServer
    {
        private readonly LuckPermsService _luckPermsService;

        public PermissionsHub(LuckPermsService luckPermsService)
        {
            _luckPermsService = luckPermsService;
        }


        public async Task<bool> CheckUserPermissionAsync(string uuid, string permission)
        {
            return await _luckPermsService.HasPermissionAsync(permission, uuid);
        }

        public async Task<bool> CheckGroupPermissionAsync(string group, string permission)
        {
            return await _luckPermsService.HasGroupNode(permission, group);
        }
    }
}
