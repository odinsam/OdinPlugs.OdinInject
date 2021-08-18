
using OdinPlugs.OdinModels.ConfigModel.RedisModels;

namespace OdinPlugs.OdinInject.Models.CacheManagerModels
{
    public class OdinCacheManagerModel
    {
        public CacheManagerModel OptCm { get; set; }
        public RedisModel OptRedis { get; set; }
    }
}