using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSRedis;
using Newtonsoft.Json;
using OdinPlugs.OdinInject.Models.RedisModels;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinRedisInject
{
    public class OdinRedis : IOdinRedis
    {
        private readonly CSRedisClient csredis;

        /// <summary>
        /// 构造 初始化 redis 对象
        /// </summary>
        /// <param name="connectionString">redis连接字符串</param>
        public OdinRedis(List<string> connectionString)
        {
            this.csredis = new CSRedisClient(null, connectionString.ToArray());
            RedisHelper.Initialization(csredis);
        }
        /// <summary>
        /// 构造 初始化 redis 对象
        /// </summary>
        /// <param name="RedisOption">构造参数对象</param>
        /// <param name="instanceName"></param>
        public OdinRedis(RedisOption option)
        {
            this.csredis = new CSRedisClient(null, option.ConnectionString.ToArray());
            RedisHelper.Initialization(csredis);
        }

        public string KeyRequired(string param) => param ?? throw new Exception("key must required");

        public bool SetObjectValue(string key, Object value, Action<OdinRedisExpireOption> action)
        {
            KeyRequired(key);
            try
            {
                OdinRedisExpireOption options = new OdinRedisExpireOption();
                action(options);
                var ts = new OdinRedisExpire().GetExpiration(options);
                return options.Expire switch
                {
                    EnumOdinRedisExpire.NoExpiration => RedisHelper.Set(key, value, -1, options.RedisExistenceNxx),
                    _ => RedisHelper.Set(key, value, ts, options.RedisExistenceNxx),
                };

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
        public async Task<bool> SetObjectValueAsync(string key, string value, Action<OdinRedisExpireOption> action)
        {
            KeyRequired(key);
            try
            {
                OdinRedisExpireOption options = new OdinRedisExpireOption();
                action(options);
                var ts = new OdinRedisExpire().GetExpiration(options);
                return options.Expire switch
                {
                    EnumOdinRedisExpire.NoExpiration => await RedisHelper.SetAsync(key, value, -1, options.RedisExistenceNxx),
                    _ => await RedisHelper.SetAsync(key, value, ts, options.RedisExistenceNxx),
                };
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
            KeyRequired(key);
            try
            {
                return RedisHelper.Get(key);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return null;
            }
        }
        /// <summary>
        /// 获取string数据
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<string> GetStringValueAsync(string key)
        {
            KeyRequired(key);
            try
            {
                return await RedisHelper.GetAsync(key);
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
            KeyRequired(key);
            try
            {
                return RedisHelper.Get<T>(key);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return default(T);
            }
        }
        /// <summary>
        /// 获取数据（对象）
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<T> GetAsync<T>(string key)
        {
            KeyRequired(key);
            try
            {
                return await RedisHelper.GetAsync<T>(key);
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
            KeyRequired(key);
            try
            {
                RedisHelper.Del(key);
                return true;
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
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
        public bool ExistsKey(string key)
        {
            KeyRequired(key);
            try
            {
                return RedisHelper.Exists(key);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(JsonConvert.SerializeObject(ex).ToJsonFormatString());
                return false;
            }
        }
    }
}