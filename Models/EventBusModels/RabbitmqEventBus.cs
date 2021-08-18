using OdinPlugs.OdinModels.ConfigModel.RabbitMQ;

namespace OdinPlugs.OdinInject.Models.EventBusModels
{
    public class RabbitmqEventBus : EventBusOptions
    {
        public RabbitMQOptions RabbitmqOptions { get; set; }
    }
}