using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;
using System.Web.Routing; 
using Loveii.Services;

namespace Loveii
{
    /// <summary>
    /// 验证是否登录Filter
    /// </summary>
    public class ValidateLoginFilter : FilterAttribute, IActionFilter
    {
        /// <summary>
        /// 在执行操作方法之前调用
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            SSO sso = SSO.Current;
            if (!sso.IsLogin)
            {
                filterContext.Result = new RedirectResult("/Admin/Login?target=" + filterContext.HttpContext.Request.RawUrl);
            }
        }

        /// <summary>
        /// 在执行操作方法后调用
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnActionExecuted(ActionExecutedContext filterContext)
        {
        }
    }
}
