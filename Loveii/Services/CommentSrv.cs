using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using Loveii.Repositories;
using Loveii.Models;

namespace Loveii.Services
{
    public class CommentSrv
    {
        public static List<Comment> GetList(int postId, bool isAll = true)
        {
            return CreateRepository.Comment.GetList(postId, isAll);
        }

        /// <summary>
        /// 更新留言为显示状态
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Result Show(int postId, int id)
        {
            return CreateRepository.Comment.Show(postId, id);
        }

        public static Result Delete(int postId, int id)
        {
            return CreateRepository.Comment.Delete(postId, id);
        }

        public static Result Add(Comment Comment)
        {
            if (Comment.postId < 0)
                return new Result(false, "被评论文章ID错误");

            if (string.IsNullOrEmpty(Comment.content))
            {
                return new Result(false, "评论内容不能为空");
            }
            else if (Comment.content.Length > 2046)
            {
                return new Result(false, "评论不能大于2046个字符");
            }

            if (string.IsNullOrEmpty(Comment.author))
                return new Result(false, "作者不能为空");

            return CreateRepository.Comment.Add(Comment);
        }
    }
}
