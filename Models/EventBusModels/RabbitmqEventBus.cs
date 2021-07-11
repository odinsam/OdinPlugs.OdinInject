using OdinPlugs.OdinInject.Models.RabbitmqModels;

namespace OdinPlugs.OdinInject.Models.EventBusModels
{
    public class RabbitmqEventBus : EventBusOptions
    {
        public RabbitMQOptions RabbitmqOptions { get; set; }
    }
}