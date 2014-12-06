using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Web.Mvc;
using Loveii.Models;
using Loveii.Services;
using Loveii.ViewModels;
using log4net;
using Loveii.Helpers;

namespace Loveii.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index(string cate, int id, HomeViewModel model)
        {
            model.s = GetData.Get<string>("s", "");

            if (string.IsNullOrEmpty(cate)) model.cate = "index";

            model.cateId = TermSrv.Get(model.cate).Item.id;

            model.PostList = PostSrv.GetPageResult(id, 10, model.cateId, model.s);

            model.NewsPostList = PostSrv.GetList();

            for (int i = 0; i < model.PostList.Item.Count; i++)
            {
                model.PostList.Item[i].Term = TermSrv.Get(model.PostList.Item[i].termId).Item;
                model.PostList.Item[i].User = UserSrv.Get(model.PostList.Item[i].uid).Item;
            }

            model.TermList = TermSrv.GetList();

            return View(model);
        }

        public ActionResult Content(int id, HomeViewModel model)
        {
            TResult<Post> result = PostSrv.Get(id);
            if (result.Successed)
            {
                model.Post = result.Item;
                model.Post.Term = TermSrv.Get(result.Item.termId).Item;
                model.Post.User = UserSrv.Get(result.Item.uid).Item;
                if (model.Post.commentStatus == "open")
                {
                    model.CommentList = CommentSrv.GetList(model.Post.id);
                }

                TResult<Post> post = PostSrv.GetNext(id);
                if (post.Successed)
                {
                    model.Post.nextId = post.Item.id;
                    model.Post.nextTitle = post.Item.title;
                }
            }
            else
            {
                return Redirect("/");
            }
            return View(model);
        }

        public ActionResult Add()
        {
            if (IsPost())
            {
                Comment model = new Comment();

                SSO sso = SSO.Current;
                if (sso.IsLogin)
                    model.author = sso.UserName;


                try
                {
                    model.postId = PostData.Get<int>("postId");
                    model.content = PostData.Get<string>("content");
                }
                catch (Exception ex)
                {
                    return Json(new Result(false, ex.Message));
                }

                model.authorIP = UtilHelper.GetIP();

                Result result = CommentSrv.Add(model);
                if (result.Successed)
                {
                    PostSrv.UpCommentCount(model.postId);
                }

                return Json(result);
            }
            return Json(new Result(false, "no"));
        } 
    }
}
