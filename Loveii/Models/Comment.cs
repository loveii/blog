using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loveii.Models
{
    public class Comment
    {
        public Comment()
        {
            this.author = "游客";
        }

        /// <summary>
        /// 自增唯一ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 被评论文章ID
        /// </summary>
        public int postId { get; set; }

        /// <summary>
        /// 评论者
        /// </summary>
        public string author { get; set; }

        /// <summary>
        /// 评论者IP
        /// </summary>
        public string authorIP { get; set; }

        /// <summary>
        /// 评论正文
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 评论是否被批准
        /// </summary>
        public byte approved { get; set; }

        /// <summary>
        /// 父评论ID
        /// </summary>
        public int parent { get; set; }

        /// <summary>
        /// 评论者用户ID（不一定存在）
        /// </summary>
        public int uid { get; set; }

        /// <summary>
        /// 评论时间
        /// </summary>
        public DateTime createTime { get; set; }

    }
}
