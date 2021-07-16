using OdinPlugs.OdinInject.Models.RedisModels;

namespace OdinPlugs.OdinInject.Models.CacheManagerModels
{
    public class OdinCacheManagerModel
    {
        public CacheManagerModel OptCm { get; set; }
        public RedisModel OptRbmq { get; set; }
    }
}