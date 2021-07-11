namespace OdinPlugs.OdinInject.Models.EventBusModels
{
    public class KafkaEventBus : EventBusOptions
    {
        public string KafkaConnectionString { get; set; }
    }
}