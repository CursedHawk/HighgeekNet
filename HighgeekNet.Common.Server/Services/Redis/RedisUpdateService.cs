using Microsoft.Extensions.Logging;

namespace HighgeekNet.Common.Server.Services.Redis
{
    public interface IRedisUpdateService
    {
        public void SendSet(string key);

        public void SendJsonSet(string key);

        public void SendExpire(string key);

        public void SendDel(string key);

        public event EventHandler<string> KeySetEvent;

        public event EventHandler<string> KeyDelEvent;

        public event EventHandler<string> KeyExpiredEvent;

        public event EventHandler<string> JsonSetEvent;
    }

    public class RedisUpdateService : IRedisUpdateService
    {
        private readonly ILogger<RedisUpdateService> _logger;

        public RedisUpdateService(ILogger<RedisUpdateService> logger)
        {
            _logger = logger;
        }

        public event EventHandler<string>? KeySetEvent;

        public event EventHandler<string>? KeyDelEvent;

        public event EventHandler<string>? KeyExpiredEvent;

        public event EventHandler<string>? JsonSetEvent;

        public void SendSet(string key)
        {
            KeySetEvent?.Invoke(this, key);
        }

        public void SendJsonSet(string key)
        {
            JsonSetEvent?.Invoke(this, key);
        }

        public void SendExpire(string key)
        {
            KeyExpiredEvent?.Invoke(this, key);
        }

        public void SendDel(string key)
        {
            KeyDelEvent?.Invoke(this, key);
        }
    }
}
