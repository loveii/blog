using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Loveii
{
    // 注意: 有关启用 IIS6 或 IIS7 经典模式的说明，
    // 请访问 http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "comment", // 路由名称 
                "comment/add", // 带有参数的 URL 
                new { controller = "Home", action = "Add" }
            );

            routes.MapRoute(
                "content", // 路由名称
                "{id}.html", // 带有参数的 URL 
                new { controller = "Home", action = "Content", id = 0 }, // 参数默认值
                new { id = @"^\d*" }
            );

            routes.MapRoute(
                "cate", // 路由名称
                "cate/{cate}/{id}", // 带有参数的 URL 
                new { controller = "Home", action = "Index", cate = "index", id = 0 } // 参数默认值 
            );
 
            routes.MapRoute(
                "Default", // 路由名称
                "{controller}/{action}/{id}", // 带有参数的 URL
                new { controller = "Home", action = "Index", id = 0 } // 参数默认值
            ); 

 
        }

        protected void Application_Start()
        {
            MvcHandler.DisableMvcResponseHeader = true;
            AreaRegistration.RegisterAllAreas();
            RegisterRoutes(RouteTable.Routes);
        }
    }
}
