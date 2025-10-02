using HighgeekNet.Blazor.Client.Services.SignalR.Snack;
using HighgeekNet.Common.Permissions;
using HighgeekNet.Common.Server.Services.Redis;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using MudBlazor;

namespace HighgeekNet.Blazor.Services.SignalR.Snack
{
    public class SnackHub : Hub<ISnackClient>, ISnackServer
    {
        /*
        public async Task SendSnackToAll(string message, Severity severity)
        {
            await Clients.All.SendAsync("SendSnack", message, severity);
        }

        public async Task SendSnackToUser(string user, string message, Severity severity)
        {
            await Clients.Caller.SendAsync("SendSnack", message, severity);
        }

        public async Task SendSnackToGroup(string group, string message, Severity severity)
        {
            await Clients.Group(group).SendAsync("SendSnack", message, severity);
        }*/

        public async Task SendSnack(string message, Severity severity)
        {
            await Clients.All.ReceiveSnack(message, severity);
        }
    }
}
