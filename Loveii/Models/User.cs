using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loveii.Models
{
    public class User
    {
        /// <summary>
        /// 自增唯一ID
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 登录名
        /// </summary>
        public string userName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string niceName { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 用户状态 默认0
        /// </summary>
        public byte status { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime createTime { get; set; }
    }
}
