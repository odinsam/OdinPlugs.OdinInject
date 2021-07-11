namespace OdinPlugs.OdinInject.Models.EventBusModels
{
    public class MongoEventBus : EventBusOptions
    {
        public string MongoConnectionString { get; set; }
    }
}