using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Loveii.Cache
{
    /// <summary>
    /// 缓存组件
    /// </summary>
    public class CacheHelper
    {
        private static ICacheStrategy _cacheStrategy;

        /// <summary>
        /// 缓存Helper
        /// </summary>
        static CacheHelper()
        {
            _cacheStrategy = new AspNetCache();
        }
        
        /// <summary>
        /// 根据缓存KEY获得一个缓存
        /// </summary>
        /// <typeparam name="T">接受数据类型</typeparam>
        /// <param name="xpath">缓存key</param>
        /// <returns></returns>
        public static T Get<T>(string xpath)
        {
            object obj = _cacheStrategy.Get(xpath);
            return obj == null ? default(T) : (T)obj;
        }

        /// <summary>
        /// 缓存一个对象
        /// 默认5分钟
        /// </summary>
        /// <param name="xpath">缓存key</param>
        /// <param name="data">缓存数据项</param>
        public static void Set(string xpath, object data)
        {
            Set(xpath, data, 300);
        }

        /// <summary>
        /// 缓存一个对象
        /// </summary>
        /// <param name="xpath">缓存key</param>
        /// <param name="data">缓存数据项</param>
        /// <param name="seconds">缓存时间（单位秒）</param>
        public static void Set(string xpath, object data, int seconds)
        {
            //缓存接口调用
            _cacheStrategy.Set(xpath, data, seconds);
        }

        /// <summary>
        /// 删除一个缓存
        /// </summary>
        /// <param name="xpath">缓存key</param>
        public static void Remove(string xpath)
        {
            _cacheStrategy.Remove(xpath);
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public static void Flush()
        {
            _cacheStrategy.Flush();
        }

        /// <summary>
        /// 是否存在某一条缓存记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Contains(string key)
        {
            return _cacheStrategy.Contains(key);
        }

        /// <summary>
        /// 删除一个列表,自动匹配后面参数
        /// </summary>
        /// <param name="ListKey"></param>
        public static void DelCacheList(string ListKey)
        {
            _cacheStrategy.DelList(ListKey);
        }
    }
}
