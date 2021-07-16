using Microsoft.Extensions.Caching.Distributed;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinRedisInject
{
    public interface IOdinRedis : IAutoInjectWithParams
    {
        /// <summary>
        /// 初始化Redis
        /// </summary>
        public void InitRedis(string connectionString, string instanceName);

        /// <summary>
        /// 添加string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="ExprireTime">过期时间 单位秒</param>
        /// <returns></returns>
        public bool SetStringValue(string key, string value, int ExprireTime = 86400);


        /// <summary>
        /// 添加string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="DistributedCacheEntryOptions">过期策略</param>
        /// <returns></returns>
        public bool SetStringValue(string key, string value, DistributedCacheEntryOptions options);
        /// <summary>
        /// 获取string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetStringValue(string key);
        /// <summary>
        /// 获取数据（对象）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key);
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key">键</param>
        public bool Remove(string key);
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="key">键</param>
        public bool Refresh(string key);
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间 单位小时</param>
        public bool Replace(string key, string value, int expireTime = 24);
        /// <summary>
        /// 判断key是否准确
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="ExprireTime">过期时间 单位秒</param>
        /// <returns></returns>
        public bool ExistsStringValue<T>(string key);
    }
}