using System.Collections.Generic;

namespace OdinPlugs.OdinInject.Models.RedisModels
{
    public class RedisOption
    {
        public List<string> ConnectionString { get; set; }
        public string InstanceName { get; set; }
    }
}