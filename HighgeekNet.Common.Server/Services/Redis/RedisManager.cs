using HighgeekNet.Common.Server.Config;
using NRedisStack.RedisStackCommands;
using StackExchange.Redis;

namespace HighgeekNet.Common.Server.Services.Redis
{
    public class RedisManager
    {
        public static readonly string host = ConfigProvider.Get("Redis:host");

        public static readonly ConnectionMultiplexer Redis = ConnectionMultiplexer.Connect(host);

        public static readonly IDatabase Database = Redis.GetDatabase();

        public static readonly IServer server = Redis.GetServers().Single();

        public static async Task<string> GetFromRedisAsync(string uuid)
        {
            return await Database.StringGetAsync(uuid);
        }
        public static string GetFromRedis(string uuid)
        {
            return Database.StringGet(uuid);
        }

        public static async Task SetInRedis(string uuid, string value)
        {
            await Database.StringSetAsync(uuid, value);
        }

        public static async Task<List<string>> GetKeysList(string pattern)
        {
            List<string> output = new List<string>();

            foreach (var key in server.Keys(pattern: pattern))
            {
                output.Add(key);
            }
            return output;
        }

        public static async Task DelFromRedis(string uuid)
        {
            await Database.KeyDeleteAsync(uuid);
        }

        public static string GetJson(string uuid)
        {
            return Database.JSON().Get(uuid).ToString();
        }
    }
}
