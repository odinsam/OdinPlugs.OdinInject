using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using OdinPlugs.OdinInject.InjectInterface;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinRedisInject
{
    public interface IOdinRedis : IAutoInjectWithParams
    {
        /// <summary>
        /// 判断参数
        /// </summary>
        /// <param name="param">param</param>
        /// <returns>param required return true,otherwais false.</returns>
        string KeyRequired(string param);


        /// <summary>
        /// set key-value:object into redis
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value type</param>
        /// <param name="action">OdinRedisExpireOption action</param>
        /// <returns></returns>
        bool SetObjectValue(string key, Object value, Action<OdinRedisExpireOption> action);


        /// <summary>
        /// set key-value:object into redis async
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value type is string</param>
        /// <param name="action">OdinRedisExpireOption action</param>
        /// <returns></returns>
        Task<bool> SetObjectValueAsync(string key, string value, Action<OdinRedisExpireOption> action);



        /// <summary>
        /// 添加string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="ExprireTime">过期时间 单位秒</param>
        /// <returns></returns>
        string GetStringValue(string key);


        /// <summary>
        /// 添加string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="DistributedCacheEntryOptions">过期策略</param>
        /// <returns></returns>
        Task<string> GetStringValueAsync(string key);
        /// <summary>
        /// 获取string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        T Get<T>(string key);
        /// <summary>
        /// 获取数据（对象）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        Task<T> GetAsync<T>(string key);
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key">键</param>
        bool Remove(string key);
        /// <summary>
        /// 判断key是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>exists return true,otherwais false</returns>
        bool ExistsKey(string key);
    }
}