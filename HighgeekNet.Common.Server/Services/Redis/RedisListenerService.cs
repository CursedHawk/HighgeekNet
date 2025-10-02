using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace HighgeekNet.Common.Server.Services.Redis
{
    public class RedisListenerService : BackgroundService
    {
        private readonly ILogger<RedisListenerService> _logger;
        private readonly IRedisUpdateService _redisUpdateService;
        private ConnectionMultiplexer _redisConnection;
        private ISubscriber _subscriber;

        public RedisListenerService(ILogger<RedisListenerService> logger, IRedisUpdateService redisUpdateService)
        {
            _logger = logger;
            _redisUpdateService = redisUpdateService;
            _redisConnection = RedisManager.Redis;
            _subscriber = _redisConnection.GetSubscriber();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                return Task.Run(() => RedisListenerAsync(stoppingToken));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(
                    $"Failed to execute RedisListenerService with exception message {ex.Message}. Good luck next round!\n Stacktrace: \n{ex.StackTrace}");
                return Task.CompletedTask;
            }
        }

        public async Task RedisListenerAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Redis Listener Service is starting.");

            await _subscriber.SubscribeAsync("*", async (channel, message) =>
            {
                var key = GetKey(channel);
                switch (key)
                {
                    case "set":
                        _redisUpdateService.SendSet(message);
                        return;
                    case "del":
                        _redisUpdateService.SendDel(message);
                        return;
                    case "expired":
                        _redisUpdateService.SendExpire(message);
                        return;
                    case "json.set":
                        _redisUpdateService.SendJsonSet(message);
                        return;
                    default:
                        return;
                }
            });

            await Task.Delay(Timeout.Infinite, stoppingToken);

            _logger.LogDebug($"Redis Listener Service is stopping.");
        }

        public static string GetKey(string channel)
        {
            var index = channel.IndexOf(':');

            if (index >= 0 && index < channel.Length - 1)
            {
                return channel[(index + 1)..];
            }

            return channel;
        }
    }
}
