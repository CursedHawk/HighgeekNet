using HighgeekNet.Blazor.Client.Services.SignalR.Snack;
using HighgeekNet.Common.Server.Services.Redis;
using Microsoft.AspNetCore.SignalR;
using MudBlazor;

namespace HighgeekNet.Blazor.Services.SignalR.Snack
{
    public interface ISnackService
    {
        public void CallFromRedis(object? sender, string key);
    }

    public class SnackService : ISnackService
    {
        private readonly IRedisUpdateService _redisUpdateService;
        private readonly IHubContext<SnackHub, ISnackClient> _snackHub;

        public SnackService(IRedisUpdateService redis, IHubContext<SnackHub, ISnackClient> snackHub)
        {
            _snackHub = snackHub;
            _redisUpdateService = redis;

            _redisUpdateService.KeySetEvent += CallFromRedis;
        }


        public async void CallFromRedis(object? sender, string key)
        {
            await _snackHub.Clients.All.ReceiveSnack(key, Severity.Success);
        }
    }
}
