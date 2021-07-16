namespace OdinPlugs.OdinInject.Models.CacheManagerModels
{
    public class CacheManagerModel
    {
        public string CacheName { get; set; }
        public int RetryTimeout { get; set; } = 100;
        public string UpdateMode { get; set; } = "Up";
        public int MaxRetries { get; set; } = 1000;
        public BackplaneModel BackPlane { get; set; }
        public LoggerFactoryModel LoggerFactory { get; set; }
        public SerializerModel Serializer { get; set; }
        public HandlesModel[] Handles { get; set; }
    }
}