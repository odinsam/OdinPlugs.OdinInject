namespace OdinPlugs.OdinInject.Models.ConsulModels
{
    public class CheckOptionModel
    {
        public int DeregisterCriticalServiceAfter { get; set; }
        public string HealthApi { get; set; }
        public int Interval { get; set; }
        public int Timeout { get; set; }
    }
}