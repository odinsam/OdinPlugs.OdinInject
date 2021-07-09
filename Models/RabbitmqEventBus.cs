using OdinPlugs.OdinInject.Models.RabbitmqModels;

namespace OdinPlugs.OdinInject.Models
{
    public class RabbitmqEventBus : EventBusOptions
    {
        public RabbitMQOptions RabbitmqOptions { get; set; }
    }
}