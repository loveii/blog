using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loveii.Models
{
    public class Term
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 分类缩略名
        /// </summary>
        public string slug { get; set; }

        /// <summary>
        /// 组ID
        /// </summary>
        public int group { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public byte order { get; set; }
    }
}
