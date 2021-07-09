namespace OdinPlugs.OdinInject.Models
{
    public class MongoEventBus : EventBusOptions
    {
        public string MongoConnectionString { get; set; }
    }
}