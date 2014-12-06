using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using log4net;

namespace Loveii
{
    public class BaseController : Controller
    {
        static ILog log = LogManager.GetLogger(typeof(BaseController));

        /// <summary>
        /// 同步间隔时间 十分钟同步一次
        /// </summary>
        internal static readonly int _syncTimer = 600000;

        /// <summary>
        /// 定时器
        /// </summary>
        internal static System.Timers.Timer _timerClock = new System.Timers.Timer();

        /// <summary>
        /// GET内容
        /// </summary>
        public static ClientDataCollection GetData = new ClientDataCollection(false);

        /// <summary>
        /// POST内容
        /// </summary>
        public static ClientDataCollection PostData = new ClientDataCollection(true);

        /// <summary>
        /// 点击数
        /// </summary>
        public static Dictionary<int, int> CLICK = new Dictionary<int, int>();

        public BaseController()
        { 
            _timerClock.Elapsed += new System.Timers.ElapsedEventHandler(OnTimer);
            _timerClock.Interval = _syncTimer;
            _timerClock.Enabled = true;
        }
        //点击数插入数据库。
        internal static void OnTimer(Object source, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                Loveii.Services.PostSrv.UpClick(CLICK);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 字符串数字类型转换成Int类型，如果转换失败返回0
        /// </summary>
        /// <param name="strId"></param>
        /// <returns></returns>
        public static int Str2Int(string strId)
        {
            int id = 0;
            int.TryParse(strId, out id);
            return id;
        }

        /// <summary>
        /// 是否为当前同域提交
        /// </summary>
        /// <returns></returns>
        public bool ValidateDomain()
        {
            if (string.IsNullOrEmpty(GlobalConfig.Instance.Domain))
                return true;

            if (Request.UrlReferrer == null)
                return false;

            return Request.UrlReferrer.Host.ToString().ToLower().Contains(GlobalConfig.Instance.Domain);
        }

        /// <summary>
        /// 当前是否为POST状态
        /// </summary>
        /// <returns></returns>
        public bool IsPost()
        {
            return Request.RequestType == "POST" ? true : false;
        }

        /// <summary>
        /// 提示信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ViewNotice(BaseViewModel model)
        {
            return View("notice", model);
        }

        /// <summary>
        /// 未登录状态的成功提示信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public ActionResult ViewOk(BaseViewModel model)
        {
            return View("ok", model);
        }

        /// <summary>
        /// 异常处理
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);

            //Write Log
            log.Error("【Exception】", filterContext.Exception);

            Uri prevUrl = filterContext.HttpContext.Request.UrlReferrer;
            if (prevUrl != null)
            {
                filterContext.Controller.ViewData["PrevUrl"] = prevUrl.ToString();
            }

            filterContext.Result = new ViewResult { ViewName = "error", ViewData = filterContext.Controller.ViewData };
            filterContext.ExceptionHandled = true;
        }
    }
}
