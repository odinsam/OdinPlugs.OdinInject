using System;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using OdinPlugs.OdinInject.Models.RedisModels;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinRedisInject
{
    public class OdinRedis : IOdinRedis
    {
        private static RedisCache _redisCache = null;
        private static RedisCacheOptions options = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="instanceName"></param>
        public OdinRedis(string connectionString, string instanceName)
        {
            options = new RedisCacheOptions();
            options.Configuration = connectionString;
            options.InstanceName = instanceName;
            _redisCache = new RedisCache(options);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="instanceName"></param>
        public OdinRedis(RedisOption option)
        {
            options = new RedisCacheOptions();
            options.Configuration = option.ConnectionString;
            options.InstanceName = option.InstanceName;
            _redisCache = new RedisCache(options);
        }
        /// <summary>
        /// 初始化Redis
        /// </summary>
        public void InitRedis(string connectionString, string instanceName)
        {
            options = new RedisCacheOptions();
            options.Configuration = connectionString;
            options.InstanceName = instanceName;
            _redisCache = new RedisCache(options);
        }
        /// <summary>
        /// 添加string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="ExprireTime">过期时间 单位秒</param>
        /// <returns></returns>
        public bool SetStringValue(string key, string value, int ExprireTime = 86400)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                _redisCache.SetString(key, value, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(ExprireTime)
                });
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return false;
            }
        }
        /// <summary>
        /// 添加string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="DistributedCacheEntryOptions">过期策略</param>
        /// <returns></returns>
        public bool SetStringValue(string key, string value, DistributedCacheEntryOptions options)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                _redisCache.SetString(key, value, options);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return false;
            }
        }
        /// <summary>
        /// 获取string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public string GetStringValue(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            try
            {
                return _redisCache.GetString(key);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return null;
            }
        }
        /// <summary>
        /// 获取数据（对象）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            string value = GetStringValue(key);
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }
            try
            {
                if (typeof(T) == typeof(string))
                {
                    return (T)Convert.ChangeType(value, typeof(T));
                }
                var obj = JsonConvert.DeserializeObject<T>(value);
                return obj;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return default(T);
            }
        }
        /// <summary>
        /// 移除数据
        /// </summary>
        /// <param name="key">键</param>
        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                _redisCache.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return false;
            }
        }
        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="key">键</param>
        public bool Refresh(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                _redisCache.Refresh(key);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return false;
            }
        }
        /// <summary>
        /// 重置数据
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expireTime">过期时间 单位小时</param>
        public bool Replace(string key, string value, int expireTime = 24)
        {
            if (Remove(key))
            {
                return SetStringValue(key, value, expireTime);
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 判断key是否准确
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="ExprireTime">过期时间 单位秒</param>
        /// <returns></returns>
        public bool ExistsStringValue<T>(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            try
            {
                var obj = Get<T>(key);
                return obj != null;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return false;
            }
        }
    }
}