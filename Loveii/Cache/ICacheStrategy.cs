using System;
using System.Collections.Generic;
using System.Text;

namespace Loveii.Cache
{
    /// <summary>
    /// 缓存接口
    /// </summary>
    public interface ICacheStrategy
    {
        /// <summary>
        /// 根据缓存key获取缓存的对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object Get(string key);

        /// <summary>
        /// 设置一个对象
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">数据</param>
        void Set(string key, object data);

        /// <summary>
        /// 设置一个对象
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">数据</param>
        /// <param name="seconds">缓存时间(秒)</param>
        void Set(string key, object data, int seconds);

        /// <summary>
        /// 移除一个缓存key
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 清除所有缓存
        /// </summary>
        void Flush();

        /// <summary>
        /// 是否存在某一条缓存记录
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Contains(string key);

        /// <summary>
        /// 删除列表
        /// </summary>
        /// <param name="ListKey"></param>
        void DelList(string ListKey);

    }
}
