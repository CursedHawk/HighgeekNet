using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using MudBlazor;
using System.Diagnostics;

namespace HighgeekNet.Blazor.Client.Services.SignalR
{
    public class SnackService : ISnackClient, IAsyncDisposable
    {
        private readonly HubConnection _hubConnection;

        private readonly NavigationManager _navigationManager;

        private readonly ISnackbar _snack;

        public SnackService(NavigationManager navigationManager, ISnackbar snackbar)
        {
            _navigationManager = navigationManager;
            _snack = snackbar;

            _hubConnection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/hubs/snack"))
            .WithAutomaticReconnect()
            .Build();

            _hubConnection.On<string, Severity>(
            nameof(ISnackClient.ReceiveSnack), 
                (message, severity) =>
            {
                _snack.Add(message, severity);
            });

        }

        public async Task StartAsync()
        {
            await _hubConnection.StartAsync();
        }

        // Implement IChatClient
        public Task ReceiveSnack(string message, Severity severity)
        {
            _snack.Add(message, severity);
            return Task.CompletedTask;
        }

        public Task SendSnack(string message, Severity severity)
        {
            return _hubConnection.InvokeAsync(nameof(ISnackServer.SendSnack), message, severity);
        }


        public bool IsConnected =>
            _hubConnection?.State == HubConnectionState.Connected;


        public async ValueTask DisposeAsync()
        {
            if (_hubConnection is not null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}
