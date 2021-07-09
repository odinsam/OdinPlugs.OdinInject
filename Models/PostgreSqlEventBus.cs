namespace OdinPlugs.OdinInject.Models
{
    public class PostgreSqlEventBus : EventBusOptions
    {
        public string PostgreSqlConnectionString { get; set; }
    }
}