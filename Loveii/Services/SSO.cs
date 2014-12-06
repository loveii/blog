using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models;
using Loveii.Helpers; 
using Loveii.Repositories;
using System.Web.Security; 

namespace Loveii.Services
{
    /// <summary>
    /// 登录用户服务类
    /// </summary>
    public class SSO
    {
        public SSO()
        {
            this.UID = 0;
            this.UserName = string.Empty;
            this.SessionKey = string.Empty;
            this.TimeStamp = DateTime.Now.Ticks.ToString();
            this.ServerTime = DateTime.Now.ToString();
        }

        /// <summary>
        /// 用户Id
        /// </summary>
        public int UID { get; private set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; private set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public string TimeStamp { get; private set; }

        /// <summary>
        /// Sessionkey
        /// </summary>
        public string SessionKey { get; private set; }

        /// <summary>
        /// 记录服务器当前时间
        /// </summary>
        public string ServerTime { get; private set; }

        /// <summary>
        /// 是否登录
        /// </summary>
        public bool IsLogin
        {
            get
            {
                return Validate();
            }
        }

        /// <summary>
        /// 当前用户sso信息，并刷新客户端Cookies
        /// </summary>
        public static SSO Current
        {
            get
            {
                SSO s = new SSO();
                s.LoadCookies(true);
                return s;
            }
        }

        /// <summary>
        /// 当前用户sso信息，不刷新客户端Cookies
        /// </summary>
        public static SSO CurrentNoRefresh
        {
            get
            {
                SSO s = new SSO();
                s.LoadCookies();
                return s;
            }
        }

        /// <summary>
        /// 登陆
        /// </summary> 
        /// <param name="userName">通行证帐号</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        public TResult<User> Login(string userName, string password)
        {
            return Login(userName, password, false);
        }

        /// <summary>
        /// 登陆
        /// </summary> 
        /// <param name="userName">通行证帐号</param>
        /// <param name="Password">密码</param>
        /// <param name="writeCookies">是否写cookies</param>
        /// <returns></returns>
        public TResult<User> Login(string userName, string password, bool writeCookies)
        {
            if (!UtilHelper.IsUserName(userName))
                return new TResult<User>(false, "用户名错误.");

            if (!UtilHelper.IsPassword(password))
                return new TResult<User>(false, "密码错误.");

            password = SecurityHelper.SHA1(password.ToLower());

            TResult<User> result = CreateRepository.User.Login(userName.ToLower(), password);

            if (result.Successed)
            {
                this.UID = result.Item.id;
                this.UserName = userName;
                this.TimeStamp = DateTime.Now.Ticks.ToString();
                this.ServerTime = DateTime.Now.ToString();
                this.SessionKey = CreateSessionkey();

                if (writeCookies)
                {
                    WriteCookies();
                }
            }

            return result;
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Logout()
        {
            CookieHelper.SetCookie("__p_uid", 0);
            CookieHelper.SetCookie("__p_un", "");
            CookieHelper.SetCookie("__p_ts", "");
            CookieHelper.SetCookie("__p_st", "");
            CookieHelper.SetCookie("__p_sk", "");
        }

        /// <summary>
        /// 数据效验
        /// </summary>
        /// <returns></returns>
        private bool Validate()
        {
            if ((DateTime.Now - Convert.ToDateTime(this.ServerTime)).Minutes > 1440)
                return false;

            if (string.IsNullOrEmpty(this.SessionKey) || this.UID < 1)
                return false;

            if (this.SessionKey == CreateSessionkey())
                return true;

            return false;
        }

        /// <summary>
        /// 生成Sessionkey
        /// </summary>
        private string CreateSessionkey()
        {
            if (this.UID < 1)
                return "";

            if (string.IsNullOrEmpty(UserName))
                return "";

            StringBuilder query = new StringBuilder("loveii.com");
            query.Append("uid:").Append(this.UID.ToString());
            query.Append("username:").Append(this.UserName);
            query.Append("timestamp:").Append(this.TimeStamp);
            return SecurityHelper.SHA1(query.ToString());
        }

        /// <summary>
        /// 加载cookies信息
        /// </summary>
        /// <param name="isWriteCookies">是否重新写入</param>
        private void LoadCookies(bool isWriteCookies = false)
        {
            this.UID = CookieHelper.GetCookie("__p_uid", 0);
            this.UserName = CookieHelper.GetCookie("__p_un", "");
            this.TimeStamp = CookieHelper.GetCookie("__p_ts", "");
            this.ServerTime = CookieHelper.GetCookie("__p_st", DateTime.Now).ToString();
            this.SessionKey = CookieHelper.GetCookie("__p_sk", "");
            if (isWriteCookies)
                WriteCookies();
        }

        /// <summary>
        /// 写cookies
        /// </summary>
        public void WriteCookies()
        {
            if (Validate())
            {
                CookieHelper.SetCookie("__p_uid", this.UID, CookieHelper.TEN_YEAR);
                CookieHelper.SetCookie("__p_un", this.UserName, CookieHelper.TEN_YEAR);
                CookieHelper.SetCookie("__p_ts", this.TimeStamp, CookieHelper.TEN_YEAR);
                CookieHelper.SetCookie("__p_st", DateTime.Now.ToString(), CookieHelper.TEN_YEAR);
                CookieHelper.SetCookie("__p_sk", this.SessionKey, CookieHelper.TEN_YEAR);
            }
        }
    }
}
