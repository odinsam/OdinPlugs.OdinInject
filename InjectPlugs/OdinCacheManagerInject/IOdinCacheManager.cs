using System;
using CacheManager.Core;
using OdinPlugs.OdinInject.InjectInterface;
using SqlSugar;
namespace OdinPlugs.OdinInject.InjectPlugs.OdinCacheManagerInject
{
    public interface IOdinCacheManager : ICacheService, IAutoInjectWithParams
    {
        void Expire(string key, string region, DateTimeOffset absoluteExpiration);

        void Expire(string key, TimeSpan slidingExpiration);

        void Expire(string key, DateTimeOffset absoluteExpiration);

        void Expire(string key, string region, TimeSpan slidingExpiration);

        void Expire(string key, string region, ExpirationMode mode, TimeSpan timeout);

        void Expire(string key, ExpirationMode mode, TimeSpan timeout);

        void NoExpire(string key);



        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>如果缓存中已存在指定项，则Add方法将不成功！</returns>
        new bool Add<T>(string key, T value);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <returns>如果缓存中已存在指定项，则Add方法将不成功！</returns>
        bool Add(string key, string value);

        /// <summary>
        /// 覆盖
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        bool Cover(string key, string value);
        /// <summary>
        /// ~ 覆盖
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        bool Cover<T>(string key, T value);
        /// <summary>
        /// update
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="newValue">newValue</param>
        /// <returns>true, or false, if the update was not successful</returns>
        bool Update(string key, string newValue);
        /// <summary>
        /// ~ update
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="newValue">newValue</param>
        /// <returns>true, or false, if the update was not successful</returns>
        bool Update<T>(string key, T newValue);
        /// <summary>
        /// delete
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>true if the key was found and removed from the cache, false otherwise.</returns>
        bool Delete(string key);

        /// <summary>
        /// Clear
        /// </summary>
        void Clear();

        /// <summary>
        /// Exists
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Returns a value indicating if the key exists in at least one cache layer configured in CacheManger, without actually retrieving it from the cache.</returns>
        bool Exists(string key);
    }
}