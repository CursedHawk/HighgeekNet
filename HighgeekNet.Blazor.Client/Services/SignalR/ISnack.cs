using Microsoft.AspNetCore.Authorization;
using MudBlazor;

namespace HighgeekNet.Blazor.Client.Services.SignalR
{
    public interface ISnackClient
    {
        Task ReceiveSnack(string message, Severity severity);
    }

    public interface ISnackServer
    {
        Task SendSnack(string message, Severity severity);
    }
}
