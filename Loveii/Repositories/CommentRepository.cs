using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Loveii.Models;
using System.Data; 
using MySql.Data.MySqlClient;
using Loveii.Cache;

namespace Loveii.Repositories
{
    public class CommentRepository
    {
        public static readonly string COMMENT_KEY = "/Comment/GetList/{0}";

        public List<Comment> GetList(int postId, bool isAll = true)
        {
            string key = string.Format(COMMENT_KEY, postId);

            string sql = "";
            if (isAll)
                sql = "SELECT * FROM li_comments WHERE postId=@postId ORDER BY createTime";
            else
                sql = "SELECT * FROM li_comments WHERE postId=@postId AND approved=1 ORDER BY createTime";

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@postId", MySqlDbType.Int32, 4, postId);


            List<Comment> list = CacheHelper.Get<List<Comment>>(key);

            if (list == null)
            {
                list = context.ExecuteList<Comment>(LoadModle);
                CacheHelper.Set(key, list, 86400);
            }
            return list;
        }
        /// <summary>
        /// 更新留言为显示状态
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Result Show(int postId, int id)
        {
            string key = string.Format(COMMENT_KEY, postId);
            string sql = " UPDATE `li_comments` SET `approved` = 1 WHERE `id` = @id";

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@id", MySqlDbType.Int32, 4, id);

            TResult<Comment> model = context.ExecuteTResult<Comment>(LoadModle);

            if (model.Successed)
                CacheHelper.Remove(key);

            return model;
        }

        public Result Delete(int postId, int id)
        {
            string key = string.Format(COMMENT_KEY, postId);
            string sql = "DELETE FROM `li_comments` WHERE `id` = @id";

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@id", MySqlDbType.Int32, 4, id);

            TResult<Comment> model = context.ExecuteTResult<Comment>(LoadModle);

            if (model.Successed)
                CacheHelper.Remove(key);

            return model;
        }

        public Result Add(Comment model)
        {
            string key = string.Format(COMMENT_KEY, model.postId);

            StringBuilder s = new StringBuilder();
            s.Append("INSERT INTO `li_comments` (`postId`,`author`,`authorIP`,`content`,`approved`,`parent`,`uid`)");
            s.Append("VALUES (@postId,@author,@authorIP,@content,@approved,@parent,@uid)");

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, s.ToString());
            context.AddParameter("@postId", MySqlDbType.Int32, 4, model.postId);
            context.AddParameter("@author", MySqlDbType.VarChar, 16, model.author);
            context.AddParameter("@authorIP", MySqlDbType.VarChar, 16, model.authorIP);
            context.AddParameter("@content", MySqlDbType.Text, -1, model.content);
            context.AddParameter("@approved", MySqlDbType.Byte, 1, model.approved);
            context.AddParameter("@parent", MySqlDbType.Int32, 4, model.parent);
            context.AddParameter("@uid", MySqlDbType.Int32, 4, model.uid);

            Result result = context.ExecuteResult();

            if (result.Successed)
                CacheHelper.Remove(key);

            return result;
        }

        #region ------Load Model------
        /// <summary>
        /// 加载下载基本信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Comment LoadModle(IDataReader dr)
        {
            Comment model = new Comment();
            model.approved = Convert.ToByte(dr["approved"]);
            model.author = dr.GetString("author");
            model.authorIP = dr.GetString("authorIP");
            model.content = dr.GetString("content");
            model.createTime = dr.GetDateTime("createTime");
            model.id = dr.GetInt32("id");
            model.parent = dr.GetInt32("parent");
            model.postId = dr.GetInt32("postId");
            model.uid = dr.GetInt32("uid");

            return model;
        }
        #endregion
    }
}
