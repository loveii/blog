using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc; 
using Loveii.Models;
using Loveii.Services;
using log4net;
using System.Text;
using Loveii.ViewModels;

namespace Loveii.Web.Controllers
{
    public class AdminController : BaseController
    {
        [ValidateLoginFilter]
        public ActionResult Index(AdminViewModel model)
        {
            int id = GetData.Get<int>("page", 0);
            model.cateId = GetData.Get<int>("termId", 1);

            model.PostList = PostSrv.GetPageResult(id, 10, model.cateId, "");

            foreach (var item in TermSrv.GetList())
            {
                model.TermSelectList.Add(new SelectListItem() { Text = item.name, Value = item.id.ToString() });
            }

            return View(model);
        }

        [ValidateInput(false)]
        [ValidateLoginFilter]
        public ActionResult Add(AdminViewModel model)
        {
            if (IsPost())
            {
                SSO sso = SSO.Current;
                bool isLogin = sso.IsLogin;
                if (!isLogin)
                    return Json(new Result(false, "登录已失效"));

                model.Post.termId = PostData.Get<int>("termId", 0);
                model.Post.title = PostData.Get<string>("title", "");
                model.Post.excerpt = PostData.Get<string>("excerpt", "");
                model.Post.content = PostData.Get<string>("content", "");
                model.Post.uid = sso.UID;

                Result result = PostSrv.Add(model.Post);
                return Json(result);
            }

            foreach (var item in TermSrv.GetList())
            {
                model.TermSelectList.Add(new SelectListItem() { Text = item.name, Value = item.id.ToString() });
            }

            return View(model);
        }

        [ValidateInput(false)]
        [ValidateLoginFilter]
        public ActionResult Modify(int id, AdminViewModel model)
        {
            if (IsPost())
            {
                SSO sso = SSO.Current;
                bool isLogin = sso.IsLogin;
                if (!isLogin)
                    return Json(new Result(false, "登录已失效"));

                model.Post = PostSrv.Get(id).Item;

                model.Post.termId = PostData.Get<int>("termId", 0);
                model.Post.title = PostData.Get<string>("title", "");
                model.Post.excerpt = PostData.Get<string>("excerpt", "");
                model.Post.content = PostData.Get<string>("content", "");
                model.Post.id = PostData.Get<int>("id", 0);
                model.Post.upTime = DateTime.Now;


                Result result = PostSrv.Modify(model.Post);
                return Json(result);
            }

            foreach (var item in TermSrv.GetList())
            {
                model.TermSelectList.Add(new SelectListItem() { Text = item.name, Value = item.id.ToString() });
            }

            model.Post = PostSrv.Get(id).Item;

            return View(model);
        }

        public ActionResult Login(AdminBaseViewModel model)
        {
            if (IsPost())
            {
                string userName = PostData.Get<string>("userName", "");
                string password = PostData.Get<string>("password", "");

                TResult<User> result = SSO.Current.Login(userName, password, true);

                if (result.Successed)
                    return Redirect("/Admin/Index");
                else
                    model.AddItem("Message", result.Message);
            }

            return View(model);
        }
    }
}
