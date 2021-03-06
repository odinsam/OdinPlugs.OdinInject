namespace OdinPlugs.OdinInject.Models.RedisModels
{
    public class RedisModel
    {
        public bool Enable { get; set; }
        public string RedisIp { get; set; }
        public int RedisPort { get; set; }
        public string RedisPassword { get; set; }
        public string Connection { get; set; }
        public int DefaultDatabase { get; set; } = 0;
        public string InstanceName { get; set; }
    }
}