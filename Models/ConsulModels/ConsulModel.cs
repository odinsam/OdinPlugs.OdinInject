namespace OdinPlugs.OdinInject.Models.ConsulModels
{
    public class ConsulModel
    {
        public bool Enable { get; set; }
        public string Protocol { get; set; }
        public string ConsulName { get; set; }
        public string ConsulIpAddress { get; set; }
        public int ConsulPort { get; set; }
        public string DataCenter { get; set; }
        /// <summary>
        /// 服务器权重
        /// </summary>
        /// <value></value>
        public int Weight { get; set; } = 50;
        public CheckOptionModel ConsulCheck { get; set; }
    }
}