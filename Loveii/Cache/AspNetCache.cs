using System;
using System.Collections;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;

namespace Loveii.Cache
{
    /// <summary>
    /// 缓存
    /// </summary>
    public class AspNetCache : ICacheStrategy
    {
        private static readonly System.Web.Caching.Cache _cache;

        /// <summary>
        /// 选择缓存
        /// </summary>
        static AspNetCache()
        {
            HttpContext context = HttpContext.Current;
            if (context != null)
            {
                _cache = context.Cache;
            }
            else
            {
                _cache = HttpRuntime.Cache;
            }
        }

        /// <summary>
        /// 根据缓存KEY获得一个缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public object Get(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                return _cache.Get(key);
            }

            return null;
        }

        /// <summary>
        /// 根据缓存KEY获得一个缓存
        /// </summary>
        /// <typeparam name="T">接受对象类型</typeparam>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public T Get<T>(string key)
        {
            object obj = Get(key);
            if (obj == null)
                return default(T);
            return (T)obj;
        }

        /// <summary>
        /// 缓存一个对象
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="data">缓存数据项</param>
        public void Set(string key, object data)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _cache.Insert(key, data);
            }
        }

        /// <summary>
        /// 缓存一个对象
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <param name="data">缓存数据项</param>
        /// <param name="seconds">缓存时间（单位秒）</param>
        public void Set(string key, object data, int seconds)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _cache.Insert(key, data, null, DateTime.Now.AddSeconds(seconds), System.Web.Caching.Cache.NoSlidingExpiration);
            }
        }

        /// <summary>
        /// 删除一个缓存
        /// </summary>
        /// <param name="key">缓存key</param>
        public void Remove(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        public void Flush()
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }

            foreach (string key in al)
            {
                _cache.Remove(key);
            }
        }

        /// <summary>
        /// 判断缓存是否存在
        /// </summary>
        /// <param name="key">缓存key</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            return _cache.Get(key) != null;
        }

        /// <summary>
        /// 删除前缀是ListKey的所有缓存
        /// </summary>
        /// <param name="ListKey">如：/cms/news/newslist</param>
        public void DelList(string ListKey)
        {
            IDictionaryEnumerator CacheEnum = _cache.GetEnumerator();
            ArrayList al = new ArrayList();
            while (CacheEnum.MoveNext())
            {
                al.Add(CacheEnum.Key);
            }
            foreach (string item in al)
            {
                if (Regex.IsMatch(item, ListKey + ".*"))
                {
                    _cache.Remove(item);
                }
            }
        }
    }
}
