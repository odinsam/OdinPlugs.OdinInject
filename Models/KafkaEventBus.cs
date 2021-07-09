namespace OdinPlugs.OdinInject.Models
{
    public class KafkaEventBus : EventBusOptions
    {
        public string KafkaConnectionString { get; set; }
    }
}