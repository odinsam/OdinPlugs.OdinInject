
using OdinPlugs.OdinModels.ConfigModel.RabbitMQ;

namespace OdinPlugs.OdinInject.Models.EventBusModels
{
    public class OdinCapEventBusOptions
    {
        public string MysqlConnectionString { get; set; } = null;
        public RabbitMQOptions RabbitmqOptions { get; set; } = null;
        public string MongoConnectionString { get; set; } = null;
        public bool UseDashboard { get; set; } = true;
    }
}