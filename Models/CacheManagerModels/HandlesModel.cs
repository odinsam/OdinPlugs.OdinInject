namespace OdinPlugs.OdinInject.Models.CacheManagerModels
{
    public class HandlesModel
    {
        public string KnownType { get; set; }
        public bool EnablePerformanceCounters { get; set; } = true;
        public bool EnableStatistics { get; set; } = true;
        public string ExpirationMode { get; set; } = "Absolute";
        public int ExpirationTimeout { get; set; }
        public bool IsBackPlaneSource { get; set; }
        public string HandleName { get; set; }
    }
}