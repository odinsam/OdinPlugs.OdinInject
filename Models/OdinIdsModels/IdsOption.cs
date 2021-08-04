namespace OdinPlugs.OdinInject.Models.OdinIdsModels
{
    public class IdsOption
    {
        /// <summary>
        /// id4 rsa 密钥文件地址 e.g xxx/xxx.rsa
        /// </summary>
        /// <value></value>
        public string RsaFilePath { get; set; }
        /// <summary>
        /// mysql 数据库连接字符串
        /// </summary>
        /// <value></value>
        public string MySqlConnectionString { get; set; }
        /// <summary>
        /// Assembly Name
        /// </summary>
        /// <value></value>
        public string MigrationsAssemblyName { get; set; }
    }
}