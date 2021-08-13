using System;
using CSRedis;
using OdinPlugs.OdinUtils.Utils.OdinTime;

namespace OdinPlugs.OdinInject.InjectPlugs.OdinRedisInject
{
    public class OdinRedisExpireOption
    {
        public EnumOdinRedisExpire Expire { get; set; }
        public long ExpireTick { get; set; }
        public RedisExistence RedisExistenceNxx { get; set; }
    }
    public class OdinRedisExpire
    {
        public TimeSpan GetExpiration(OdinRedisExpireOption options)
        {
            return options.Expire switch
            {
                EnumOdinRedisExpire.AbsoluteExpiration => AbsoluteExpiration(options.ExpireTick),
                EnumOdinRedisExpire.SlidingExpiration => SlidingExpiration(options.ExpireTick),
                _ => NoExpiration()
            };
        }
        private TimeSpan NoExpiration()
        {
            return TimeSpan.Zero;
        }

        /// <summary>
        /// 绝对过期策略
        /// </summary>
        /// <param name="unixTime">过期时间 unixtime时间戳</param>
        /// <returns>Absolute Expiration timespan</returns>
        private TimeSpan AbsoluteExpiration(long unixTime)
        {
            return new TimeSpan(unixTime - UnixTimeHelper.GetUnixDateTime());
        }

        /// <summary>
        /// 相对过期策略
        /// </summary>
        /// <param name="Expire">相对过期时间 单位 秒</param>
        /// <returns>Sliding Expiration timespan</returns>
        private TimeSpan SlidingExpiration(long expire)
        {
            return new TimeSpan(UnixTimeHelper.GetUnixDateTime() + expire);
        }
    }
}