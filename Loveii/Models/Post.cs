using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loveii.Models
{
    public class Post
    {
        public Post()
        {
            this.title = string.Empty;
            this.excerpt = string.Empty;
            this.content = string.Empty;
            this.status = "publish";
            this.password = string.Empty;
            this.commentStatus = "open";
            this.Term = new Term();
            this.User = new User();
            this.nextTitle = string.Empty;
        }

        /// <summary>
        /// 自增ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int uid { get; set; }

        /// <summary>
        /// 类型ID
        /// </summary>
        public int termId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 摘录
        /// </summary>
        public string excerpt { get; set; }

        /// <summary>
        /// 正文
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 文章状态  publish
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 文章密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 排序ID
        /// </summary>
        public int order { get; set; }

        /// <summary>
        /// 评论总数
        /// </summary>
        public int commentCount { get; set; }

        /// <summary>
        /// 评论状态
        /// </summary>
        public string commentStatus { get; set; }

        /// <summary>
        /// 点击率
        /// </summary>
        public int click { get; set; }

        /// <summary>
        /// 发布时间
        /// </summary>
        public DateTime upTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime createTime { get; set; }

        /// <summary>
        /// 分类实体
        /// </summary>
        public Term Term { get; set; }

        /// <summary>
        /// 用户实体
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// 下一页
        /// </summary>
        public int nextId { get; set; }

        /// <summary>
        /// 下一页标题
        /// </summary>
        public string nextTitle { get; set; }

    }
}
