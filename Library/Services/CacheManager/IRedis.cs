namespace Services.CacheManager
{
    public interface IRedis
    {
        void SetInMin<T>(string key, int expireTime, T cacheItem, int database = 0, byte redisServer = 1);
        T Get<T>(string key, int database = 0, byte redisServer = 1);
        bool IsExist(string key, int database = 0, byte redisServer = 1);

        //void Set<T>(string key, int expireTime, T cacheItem, int database = 0, byte redisServer = 1);
        //void Set<T>(string key, TimeSpan expireTime, T cacheItem, int database = 0, byte redisServer = 1);
        //T AddOrGetExisting<T>(string key, int expireTime, Func<T> valueFactory, int database = 0, byte redisServer = 1);
        //void Remove(string key, int database = 0, byte redisServer = 1);
        //void RemoveByPatterns(string pattern = "", int database = 0, byte redisServer = 1);
        //IList<T> GetAllkeysData<T>(string pattern, int database = 0, byte redisServer = 1);
        //IList<string> GetAllkeys(string pattern, int database = 0, byte redisServer = 1);
    }
}
