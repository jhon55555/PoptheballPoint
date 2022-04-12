using Services.Common;
using StackExchange.Redis;
using System;

namespace Services.CacheManager
{
    public class RedisCon
    {
        private readonly Lazy<ConnectionMultiplexer> lazyLocal;
        private readonly Lazy<ConnectionMultiplexer> lazyCasino;
        private readonly Lazy<ConnectionMultiplexer> lazyTrader;
        private readonly Config _config;
        //private static readonly Lazy<ConnectionMultiplexer> lazy1;
        //private static readonly Lazy<ConnectionMultiplexer> lazy2;
        //private static readonly Lazy<ConnectionMultiplexer> lazy3;
        //private static readonly Lazy<ConnectionMultiplexer> lazy4;

        public RedisCon(Config config)
        {
            _config = config;
            lazyLocal = new Lazy<ConnectionMultiplexer>(() =>
         {
             return ConnectionMultiplexer.Connect(new ConfigurationOptions
             {
                 AbortOnConnectFail = false,
                 EndPoints = { _config.RedisLocal },
                 //Password = ConfigItems.RedisPasswordLocal,
                 AllowAdmin = true
             });
         });
            lazyCasino = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    EndPoints = { _config.RedisLocal },
                    //Password = ConfigItems.RedisPasswordGame,
                    AllowAdmin = true
                });
            });
        }

        public ConnectionMultiplexer ConnLocal
        {
            get
            {
                return lazyLocal.Value;
            }
        }
        public ConnectionMultiplexer ConnCasino
        {
            get
            {
                return lazyCasino.Value;
            }
        }
        public ConnectionMultiplexer ConnTrader
        {
            get
            {
                return lazyTrader.Value;
            }
        }
        //public static ConnectionMultiplexer Redis1
    }
}