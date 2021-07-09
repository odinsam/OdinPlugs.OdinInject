using OdinPlugs.OdinInject.Models.RabbitmqModels;

namespace OdinPlugs.OdinInject.Models
{
    public class OdinCapEventBusOptions
    {
        public MySqlEventBus MySqlEventBusOptions { get; set; }
        public RabbitmqEventBus RabbitmqEventBusOptions { get; set; }
        public MongoEventBus MongoEventBusOptions { get; set; }
        public KafkaEventBus KafkaEventBusOptions { get; set; }
        public AzureServiceEventBus AzureServiceEventBusOptions { get; set; }
        public PostgreSqlEventBus PostgreSqlEventBusOptions { get; set; }
        public bool UseDashboard { get; set; }
    }
}