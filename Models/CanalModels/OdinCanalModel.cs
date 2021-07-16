using Newtonsoft.Json.Linq;

namespace OdinPlugs.OdinInject.Models.CanalModels
{
    public class OdinCanalModel
    {
        /// <summary>
        /// data内为获取到的增量数据 key是数据库对应的字段 value是数据库的值
        /// </summary>
        /// <value></value>
        public JObject[] data { get; set; }
        /// <summary>
        /// 库名
        /// </summary>
        /// <value></value>
        public string database { get; set; }
        public long es { get; set; }
        public int id { get; set; }
        public bool isDdl { get; set; }
        /// <summary>
        /// 字段对应mysql的数据类型
        /// </summary>
        /// <value></value>
        public JObject mysqlType { get; set; }
        /// <summary>
        /// 如果是 update 操作这里会是更新前的数据
        /// </summary>
        /// <value></value>
        public JObject[] old { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        /// <value></value>
        public string[] pkNames { get; set; }
        public string sql { get; set; }
        public JObject sqlType { get; set; }
        /// <summary>
        /// 表名
        /// </summary>
        /// <value></value>
        public string table { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        /// <value></value>
        public long ts { get; set; }
        /// <summary>
        /// 操作类型
        /// </summary>
        /// <value></value>
        public string type { get; set; }

    }
}