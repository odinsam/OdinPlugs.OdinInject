namespace OdinPlugs.OdinInject.Models.RabbitmqModels
{
    public class RabbitMQOptions
    {
        public RabbitMQAccount Account { get; set; }
        public string[] HostNames { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        public ExchangeModel[] Exchanges { get; set; }
    }
}