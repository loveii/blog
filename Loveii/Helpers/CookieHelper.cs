using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace Loveii.Helpers
{
    public class CookieHelper
    {
        public const int TEN_YEAR = 10 * 365 * 24 * 60;

        /// <summary>
        /// 获得cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int GetCookie(string name, int defaultValue)
        {
            return UtilHelper.ParseInt(GetCookie(name, defaultValue.ToString()), defaultValue);
        }

        /// <summary>
        /// 获得cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static string GetCookie(string name, string defaultValue)
        {
            if (HttpContext.Current == null)
                return "";

            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];
            if (httpCookie != null)
                return httpCookie.Value;
            return defaultValue;

        }

        /// <summary>
        /// 获得cookie值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime GetCookie(string name, DateTime defaultValue)
        {
            if (HttpContext.Current == null)
                return DateTime.MinValue;

            HttpCookie httpCookie = HttpContext.Current.Request.Cookies[name];

            if (httpCookie != null)
            {
                try
                {
                    DateTime dtResult;
                    if (DateTime.TryParse(httpCookie.Value, out dtResult))
                        return dtResult;
                    return defaultValue;
                }
                catch
                {
                    return defaultValue;
                }
            }

            return defaultValue;
        }


        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static void SetCookie(string name, int value)
        {
            SetCookie(name, value.ToString());
        }

        /// <summary>
        /// 设置Cookies,默认15分钟
        /// </summary>
        /// <param name="name">cookies名称</param>
        /// <param name="value">cookies值</param>
        public static void SetCookie(string name, string value)
        {
            SetCookie(name, value, 15);
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="name">cookies名称</param>
        /// <param name="value">cookies值</param>
        /// <param name="expires">分钟</param>
        public static void SetCookie(string name, int value, int expires)
        {
            SetCookie(name, value.ToString(), expires);
        }

        /// <summary>
        /// 设置Cookies
        /// </summary>
        /// <param name="name">cookies名称</param>
        /// <param name="value">cookies值</param>
        /// <param name="expires">分钟</param>
        public static void SetCookie(string name, string value, int expires)
        {
            if (HttpContext.Current != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[name];
                if (cookie == null || cookie.Value == null)
                    cookie = new HttpCookie(name, value);
                cookie.Domain = GetDomain();
                cookie.Value = value;
                cookie.Expires = DateTime.Now.AddMinutes(expires);
                HttpContext.Current.Response.Cookies.Add(cookie);
            }
        }

        /// <summary>
        /// 获取一级域名
        /// </summary>
        /// <returns></returns>
        private static string GetDomain()
        {
            if (HttpContext.Current != null)
            {
                string domain = HttpContext.Current.Request.Url.DnsSafeHost;

                string[] domainArray = domain.Split('.');

                switch (domainArray.Count())
                {
                    case 2:
                        return domain;
                    case 3:
                        return domainArray[1] + "." + domainArray[2];
                    case 4:
                        return domain;
                    default:
                        return "";
                }
            }
            return "";
        }
    }
}
