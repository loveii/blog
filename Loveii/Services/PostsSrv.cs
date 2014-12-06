using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models;
using Loveii.Repositories;
using Loveii.Helpers;

namespace Loveii.Services
{
    public class PostSrv
    {
        public static TResult<Post> Get(int id)
        {
            return CreateRepository.Post.Get(id);
        }

        public static TResult<Post> GetNext(int id)
        {
            return CreateRepository.Post.GetNext(id);
        }

        public static PageResult<Post> GetPageResult(int pageIndex, int pageSize, int termId, string s)
        {
            return CreateRepository.Post.GetPageResult(pageIndex, pageSize, termId, s);
        }

        public static List<Post> GetList()
        {
            return CreateRepository.Post.GetList();
        }

        public static Result Add(Post model)
        {
            if (string.IsNullOrEmpty(model.password))
                model.password = SecurityHelper.SHA1(model.password).ToLower();
            return CreateRepository.Post.Add(model);
        }

        public static Result Modify(Post model)
        {
            if (string.IsNullOrEmpty(model.password))
                model.password = SecurityHelper.SHA1(model.password).ToLower();
            return CreateRepository.Post.Modify(model);
        }

        public static Result UpCommentCount(int id)
        {
            if (id < 0)
                return new Result(false, "文章ID错误");
            return CreateRepository.Post.UpCommentCount(id);
        }

        public static void UpClick(Dictionary<int, int> clink)
        {
            CreateRepository.Post.UpClick(clink);
        }

    }
}
