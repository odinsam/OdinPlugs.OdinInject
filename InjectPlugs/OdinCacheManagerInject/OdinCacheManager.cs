using System;
using System.Collections.Generic;
using System.Linq;
using CacheManager.Core;
using Newtonsoft.Json;
using OdinPlugs.OdinInject.Models.CacheManagerModels;
using OdinPlugs.OdinInject.Models.RedisModels;
using OdinPlugs.OdinUtils.OdinExtensions.BasicExtensions.OdinString;
using SqlSugar;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinCacheManagerInject
{
    public class OdinCacheManager : IOdinCacheManager
    {
        #region 单例创建cache缓存对象
        private static ICacheManager<Object> cacheManager;
        public OdinCacheManager(OdinCacheManagerModel option)
        {
            var cmOptions = option.OptCm;
            var rbmqOptions = option.OptRbmq;
            if (cacheManager == null)
            {
                var sysRuntimeCacheOpt = cmOptions.Handles.Where(s => s.KnownType == "SystemRuntime").Single();
                var redisCacheOpt = cmOptions.Handles.Where(s => s.KnownType == "Redis").Single();
                cacheManager = CacheFactory.Build(cmOptions.CacheName, settings =>
                {
                    settings.WithUpdateMode(cmOptions.UpdateMode.ToEnum<CacheUpdateMode>())
                        // .WithSystemRuntimeCacheHandle(sysRuntimeCacheOpt.HandleName, sysRuntimeCacheOpt.IsBackplaneSource)//内存缓存Handle
                        // .WithExpiration(sysRuntimeCacheOpt.ExpirationMode.ToEnum<ExpirationMode>(), TimeSpan.FromSeconds(sysRuntimeCacheOpt.ExpirationTimeout))
                        //.And
                        .WithRedisConfiguration(redisCacheOpt.HandleName, config => //Redis缓存配置
                        {
                            config.WithAllowAdmin()
                                .WithPassword(rbmqOptions.RedisPassword)
                                .WithDatabase(rbmqOptions.DefaultDatabase)
                                .WithEndpoint(rbmqOptions.RedisIp, rbmqOptions.RedisPort);
                        })
                        .WithJsonSerializer()
                        .WithMaxRetries(cmOptions.MaxRetries) //尝试次数
                        .WithRetryTimeout(cmOptions.RetryTimeout) //尝试超时时间
                        .WithRedisBackplane(cmOptions.BackPlane.KnownType)
                        .WithRedisCacheHandle(redisCacheOpt.HandleName, redisCacheOpt.IsBackPlaneSource) //redis缓存handle
                        .WithExpiration(redisCacheOpt.ExpirationMode.ToEnum<ExpirationMode>(), TimeSpan.FromSeconds(sysRuntimeCacheOpt.ExpirationTimeout));
                });
            }
        }
        #endregion


        /// <summary>
        /// ~ Select
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>The value being stored in the cache for the given key.</returns>
        public T Get<T>(string key)
        {
            return cacheManager.Get<T>(key);
        }

        /// <summary>
        /// ~ 新增
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>如果缓存中已存在指定项，则Add方法将不成功！</returns>
        public bool Add(string key, string value)
        {
            return cacheManager.Add(key, value);
        }

        /// <summary>
        /// ~ 新增
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>如果缓存中已存在指定项，则Add方法将不成功！</returns>
        public bool Add<T>(string key, T value)
        {
            return cacheManager.Add(key, value);
        }

        /// <summary>
        /// ~ 覆盖
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public bool Cover(string key, string value)
        {
            try
            {
                cacheManager.Put(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ~ 覆盖
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        public bool Cover<T>(string key, T value)
        {
            try
            {
                cacheManager.Put(key, value);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// ~ update
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="newValue">newValue</param>
        /// <returns>true, or false, if the update was not successful</returns>
        public bool Update(string key, string newValue)
        {
            return cacheManager.Update(key, updateValue => newValue) != null;
        }
        /// <summary>
        /// ~ update
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="newValue">newValue</param>
        /// <returns>true, or false, if the update was not successful</returns>
        public bool Update<T>(string key, T newValue)
        {
            return cacheManager.Update(key, updateValue => newValue) != null;
        }

        /// <summary>
        /// ~ delete
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>true if the key was found and removed from the cache, false otherwise.</returns>
        public bool Delete(string key)
        {
            return cacheManager.Remove(key);
        }

        /// <summary>
        /// ~ Clear
        /// </summary>
        public void Clear()
        {
            cacheManager.Clear();
        }

        /// <summary>
        /// ~ Exists
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Returns a value indicating if the key exists in at least one cache layer configured in CacheManger, without actually retrieving it from the cache.</returns>
        public bool Exists(string key)
        {
            return cacheManager.Exists(key);
        }

        void ICacheService.Add<V>(string key, V value)
        {
            this.Add(key, value);
        }

        public void Add<V>(string key, V value, int cacheDurationInSeconds)
        {
            this.Add(key, value);
        }

        public bool ContainsKey<V>(string key)
        {
            return this.Exists(key);
        }

        public IEnumerable<string> GetAllKey<V>()
        {
            return null;
        }

        public void Remove<V>(string key)
        {
            this.Delete(key);
        }

        public V GetOrCreate<V>(string cacheKey, Func<V> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (this.Exists(cacheKey))
                return this.Get<V>(cacheKey);
            else
            {
                var result = create();
                cacheManager.Add(cacheKey, result);
                return result;
            }
        }

        public void Expire(string key, string region, DateTimeOffset absoluteExpiration)
        {
            cacheManager.Expire(key, region, absoluteExpiration);
        }

        public void Expire(string key, TimeSpan slidingExpiration)
        {
            cacheManager.Expire(key, slidingExpiration);
        }

        public void Expire(string key, DateTimeOffset absoluteExpiration)
        {
            cacheManager.Expire(key, absoluteExpiration);
        }

        public void Expire(string key, string region, TimeSpan slidingExpiration)
        {
            cacheManager.Expire(key, region, slidingExpiration);
        }

        public void Expire(string key, string region, ExpirationMode mode, TimeSpan timeout)
        {
            cacheManager.Expire(key, region, mode, timeout);
        }

        public void Expire(string key, ExpirationMode mode, TimeSpan timeout)
        {
            cacheManager.Expire(key, mode, timeout);
        }

        public void NoExpire(string key)
        {
            cacheManager.Expire(key, ExpirationMode.None, TimeSpan.Zero);
        }
    }
}