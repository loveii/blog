using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loveii.Models
{
    public class Link
    {
        public Link()
        {
            this.name = string.Empty;
            this.url = string.Empty;
            this.logo = string.Empty;
        }

        /// <summary>
        /// 自增ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 连接名称
        /// </summary>
        public string name { get; set; } 

        /// <summary>
        /// 连接地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 图片logo
        /// </summary>
        public string logo { get; set; } 
        
        /// <summary>
        /// 类型1.友情连接 2.收藏连接
        /// </summary>
        public byte typeId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public byte order { get; set; }
    }
}
