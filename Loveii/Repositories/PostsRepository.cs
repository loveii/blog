using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Loveii.Models;
using MySql.Data.MySqlClient;
using System.Text;
using Loveii.Cache;

namespace Loveii.Repositories
{
    public class PostRepository
    {
        public static readonly string POSTLIST_KEY = "/Post/List";
        public static readonly string PAGERESULT_KEY = "/PageResult/{0}/{1}/{2}/{3}";
        public static readonly string POST_KEY = "/Post/Get/{0}";
        public static readonly string POSTNEXT_KEY = "/Post/NEXT/Get/{0}";

        public static readonly string SQLLIST = " `id`,`uid`,`termId`,`title`,`excerpt`,`status`,`password`,`order`,`commentCount`,`commentStatus`,`click`,`upTime`,`createTime` ";

        /// <summary>
        /// 获得游戏客户端下载列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public TResult<Post> Get(int id)
        {
            string sql = "SELECT * FROM li_posts WHERE id=@id";
            string key = string.Format(POST_KEY, id);

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@id", MySqlDbType.Int32, 4, id);

            TResult<Post> model = CacheHelper.Get<TResult<Post>>(key);
            if (model == null)
            {
                model = context.ExecuteTResult<Post>(LoadModle);
                CacheHelper.Set(key, model, 86400);
            }
            if (model.Item.id > 0)
            {
                model.Item.click = model.Item.click + 1;
                BaseController.CLICK[model.Item.id] = model.Item.click;
            }

            return model;
        }

        public void UpClick(Dictionary<int, int> clink)
        {
            foreach (var item in clink)
            {
                string sql = string.Format("UPDATE `li_posts` SET `click` = {0} WHERE `id` = {1}", item.Value, item.Key);
                MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
                context.ExecuteResult(); 
            }
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public PageResult<Post> GetPageResult(int pageIndex, int pageSize, int termId, string s)
        {
            string rowSql = string.Empty;
            string pageSql = string.Empty;
            int index = 0;
            if (pageIndex > 0) index = pageIndex - 1;
            index = index * pageSize;
            string key = string.Format(PAGERESULT_KEY, pageIndex, pageSize, termId, s);

            if (!string.IsNullOrEmpty(s))
            {
                rowSql = "SELECT COUNT(1) FROM li_posts WHERE title LIKE '%" + s + "%' ";
                pageSql = "SELECT " + SQLLIST + " FROM li_posts WHERE title LIKE '%" + s + "%' ORDER BY ID DESC Limit @PageIndex,@PageSize";
            }
            else if (termId == 1)
            {
                rowSql = "SELECT COUNT(1) FROM li_posts";
                pageSql = "SELECT " + SQLLIST + " FROM li_posts ORDER BY ID DESC Limit @PageIndex,@PageSize";
            }
            else
            {
                rowSql = "SELECT COUNT(1) FROM li_posts WHERE termId=" + termId;
                pageSql = "SELECT " + SQLLIST + " FROM li_posts WHERE termId=@TermId ORDER BY ID DESC Limit @PageIndex,@PageSize";
            }

            MySqlContext rowContext = new MySqlContext(CreateRepository.Loveii_KEY, rowSql);
            int rowCount = Convert.ToInt32(rowContext.ExecuteScalar());

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, pageSql);
            context.AddParameter("@TermId", MySqlDbType.Int32, 4, termId);
            context.AddParameter("@PageIndex", MySqlDbType.Int32, 4, index);
            context.AddParameter("@PageSize", MySqlDbType.Int32, 4, pageSize);
            context.AddParameter("@RowCount", MySqlDbType.Int32, 4, rowCount);
            context.AddParameter("@Return", MySqlDbType.Int32, 4, ParameterDirection.ReturnValue);
            context.AddParameter("@ChvMsg", MySqlDbType.VarChar, 64, ParameterDirection.Output);

            PageResult<Post> list = CacheHelper.Get<PageResult<Post>>(key);
            if (list == null)
            {
                list = context.ExecutePageResult<Post>(pageIndex, pageSize, rowCount, LoadListModle);
                CacheHelper.Set(key, list, 86400);
            }
            else
            {
                for (int i = 0; i < list.Item.Count; i++)
                {
                    if (BaseController.CLICK.Keys.Contains(list.Item[i].id))
                    list.Item[i].click = BaseController.CLICK[list.Item[i].id];
                }
            }

            return list;
        }

        public List<Post> GetList()
        {
            string sql = "SELECT " + SQLLIST + " FROM li_posts ORDER BY ID DESC Limit 10";
            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);

            List<Post> list = CacheHelper.Get<List<Post>>(POSTLIST_KEY);
            if (list == null)
            {
                list = context.ExecuteList<Post>(LoadListModle);
                CacheHelper.Set(POSTLIST_KEY, list, 86400);
            }

            return list;
        }

        public TResult<Post> GetNext(int id)
        {
            //SELECT id,title FROM li_posts WHERE id>5 LIMIT 0,1;
            string sql = "SELECT * FROM li_posts WHERE id<@id ORDER BY id DESC LIMIT 0,1";
            string key = string.Format(POSTNEXT_KEY, id);

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@id", MySqlDbType.Int32, 4, id);

            TResult<Post> model = CacheHelper.Get<TResult<Post>>(key);
            if (model == null)
            {
                model = context.ExecuteTResult<Post>(LoadModle);
                CacheHelper.Set(key, model, 86400);
            }
            return model;
        }

        public Result Add(Post model)
        {
            StringBuilder s = new StringBuilder();
            s.Append("INSERT INTO li_posts (uid,termId,title,excerpt,content,`status`,`password`,`order`,commentCount,commentStatus)");
            s.Append("VALUES (@uid,@termId,@title,@excerpt,@content,@status,@password,@order,@commentCount,@commentStatus)");

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, s.ToString());
            context.AddParameter("@uid", MySqlDbType.Int32, 4, model.uid);
            context.AddParameter("@termId", MySqlDbType.Int32, 4, model.termId);
            context.AddParameter("@title", MySqlDbType.Text, -1, model.title);
            context.AddParameter("@excerpt", MySqlDbType.Text, -1, model.excerpt);
            context.AddParameter("@content", MySqlDbType.LongText, -1, model.content);
            context.AddParameter("@status", MySqlDbType.VarChar, 20, model.status);
            context.AddParameter("@password", MySqlDbType.VarChar, 40, model.password);
            context.AddParameter("@order", MySqlDbType.Int32, 4, model.order);
            context.AddParameter("@commentCount", MySqlDbType.Int32, 4, model.commentCount);
            context.AddParameter("@commentStatus", MySqlDbType.VarChar, 20, model.commentStatus);

            Result result = context.ExecuteResult();

            return result;
        }

        public Result Modify(Post model)
        {
            string key = string.Format(POST_KEY, model.id);

            StringBuilder s = new StringBuilder();
            s.Append("UPDATE `li_posts` SET `termId`=@termId,`title`=@title,`excerpt`=@excerpt,`content`=@content,`upTime`=@upTime ");
            s.Append("WHERE `id`=@id");

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, s.ToString());
            context.AddParameter("@termId", MySqlDbType.Int32, 4, model.termId);
            context.AddParameter("@title", MySqlDbType.Text, -1, model.title);
            context.AddParameter("@excerpt", MySqlDbType.Text, -1, model.excerpt);
            context.AddParameter("@content", MySqlDbType.LongText, -1, model.content);
            context.AddParameter("@upTime", MySqlDbType.DateTime, 8, model.upTime);
            context.AddParameter("@id", MySqlDbType.Int32, 4, model.id);

            Result result = context.ExecuteResult();

            if (result.Successed)
                CacheHelper.Flush();

            return result;
        }


        public Result UpCommentCount(int id)
        {
            string rowSql = "SELECT COUNT(1) FROM `li_comments` WHERE `postId` =" + id;
            MySqlContext rowContext = new MySqlContext(CreateRepository.Loveii_KEY, rowSql);
            int rowCount = Convert.ToInt32(rowContext.ExecuteScalar());

            StringBuilder s = new StringBuilder();
            s.Append("UPDATE `li_posts` SET `commentCount` = @rowCount WHERE `id` =" + id);

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, s.ToString());
            context.AddParameter("@rowCount", MySqlDbType.Int32, 4, rowCount);
            Result result = context.ExecuteResult();

            return result;
        }

        #region ------Load Model------
        /// <summary>
        /// 加载下载基本信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Post LoadListModle(IDataReader dr)
        {
            Post model = new Post();
            model.commentCount = dr.GetInt32("commentCount");
            model.commentStatus = dr.GetString("commentStatus");
            model.createTime = dr.GetDateTime("createTime");
            model.excerpt = dr.GetString("excerpt");
            model.id = dr.GetInt32("id");
            model.order = dr.GetInt32("order");
            model.password = dr.GetString("password");
            model.status = dr.GetString("status");
            model.title = dr.GetString("title");
            model.click = dr.GetInt32("click");
            model.uid = dr.GetInt32("uid");
            model.upTime = dr.GetDateTime("upTime");
            model.termId = dr.GetInt32("termId");

            return model;
        }

        private Post LoadModle(IDataReader dr)
        {
            Post model = new Post();
            model.commentCount = dr.GetInt32("commentCount");
            model.commentStatus = dr.GetString("commentStatus");
            model.content = dr.GetString("content");
            model.createTime = dr.GetDateTime("createTime");
            model.excerpt = dr.GetString("excerpt");
            model.id = dr.GetInt32("id");
            model.order = dr.GetInt32("order");
            model.password = dr.GetString("password");
            model.status = dr.GetString("status");
            model.title = dr.GetString("title");
            model.click = dr.GetInt32("click");
            model.uid = dr.GetInt32("uid");
            model.upTime = dr.GetDateTime("upTime");
            model.termId = dr.GetInt32("termId");

            return model;
        }
        #endregion
    }
}