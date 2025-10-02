using HighgeekNet.Blazor.Client.Services.SignalR.Permissions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Diagnostics;

namespace HighgeekNet.Blazor.Client.Services.SignalR.Permissions
{
    public class PermissionService : IPermissionClient, IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;

        private readonly NavigationManager _navigationManager;

        public bool IsConnected =>
            _hubConnection?.State == HubConnectionState.Connected;

        public PermissionService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;

            _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/hubs/perms"))
            .WithAutomaticReconnect()
            .Build();


        }

        public async Task StartAsync()
        {
            await _hubConnection.StartAsync();
        }


        public async Task<bool> CheckUserPermissionAsync(string uuid, string permission)
        {
            return await _hubConnection.InvokeAsync<bool>(nameof(IPermissionServer.CheckUserPermissionAsync), uuid, permission);
        }

        public async Task<bool> CheckGroupPermissionAsync(string group, string permission)
        {
            return await _hubConnection.InvokeAsync<bool>(nameof(IPermissionServer.CheckGroupPermissionAsync), group, permission);
        }


        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
