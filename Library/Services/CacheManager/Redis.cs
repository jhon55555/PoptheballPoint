using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;

namespace Services.CacheManager
{

    public enum RedisServer
    {
        Local = 1,
        Casino = 2,
        Trader = 3
        //Redis2 = 4,
        //Redis3 = 5,
        //Redis4 = 6
    }

    public class Redis : IRedis
    {
        private readonly RedisCon RedisCon;
        public Redis(RedisCon _RedisCon)
        {
            RedisCon = _RedisCon;
        }
        #region Methods
        public void SetInMin<T>(string key, int expireTime, T cacheItem, int database = 0, byte redisServer = 1)
        {

            IDatabase db;

            if ((byte)RedisServer.Casino == redisServer)
                db = RedisCon.ConnCasino.GetDatabase(database);
            else if ((byte)RedisServer.Trader == redisServer)
                db = RedisCon.ConnTrader.GetDatabase(database);
            //else if ((byte)RedisServer.Redis2 == redisServer)
            //    db = RedisConnection.Redis2.GetDatabase(database);
            //else if ((byte)RedisServer.Redis3 == redisServer)
            //    db = RedisConnection.Redis3.GetDatabase(database);
            //else if ((byte)RedisServer.Redis4 == redisServer)
            //    db = RedisConnection.Redis4.GetDatabase(database);
            else
                db = RedisCon.ConnLocal.GetDatabase(database);

            db.StringSet(key, JsonConvert.SerializeObject(cacheItem), TimeSpan.FromMinutes(expireTime));
        }
        public T Get<T>(string key, int database = 0, byte redisServer = 1)
        {
            IDatabase db;

            if ((byte)RedisServer.Casino == redisServer)
                db = RedisCon.ConnCasino.GetDatabase(database);
            else if ((byte)RedisServer.Trader == redisServer)
                db = RedisCon.ConnTrader.GetDatabase(database);
            //else if ((byte)RedisServer.Redis2 == redisServer)
            //    db = RedisConnection.Redis2.GetDatabase(database);
            //else if ((byte)RedisServer.Redis3 == redisServer)
            //    db = RedisConnection.Redis3.GetDatabase(database);
            //else if ((byte)RedisServer.Redis4 == redisServer)
            //    db = RedisConnection.Redis4.GetDatabase(database);
            else
                db = RedisCon.ConnLocal.GetDatabase(database);

            return JsonConvert.DeserializeObject<T>(db.StringGet(key));
        }
        public bool IsExist(string key, int database = 0, byte redisServer = 1)
        {
            IDatabase db;

            if ((byte)RedisServer.Casino == redisServer)
                db = RedisCon.ConnCasino.GetDatabase(database);
            else if ((byte)RedisServer.Trader == redisServer)
                db = RedisCon.ConnTrader.GetDatabase(database);
            //else if ((byte)RedisServer.Redis2 == redisServer)
            //    db = RedisConnection.Redis2.GetDatabase(database);
            //else if ((byte)RedisServer.Redis3 == redisServer)
            //    db = RedisConnection.Redis3.GetDatabase(database);
            //else if ((byte)RedisServer.Redis4 == redisServer)
            //    db = RedisConnection.Redis4.GetDatabase(database);
            else
                db = RedisCon.ConnLocal.GetDatabase(database);

            if (db.KeyExists(key))
                return true;
            else
                return false;
        }

        #endregion
    }
}
