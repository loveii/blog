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
    public class TermRepository
    {
        public static readonly string TERMLIST_KEY = "/Term/List";
        public static readonly string TERM_KEY = "/Term/Get/{0}";

        public List<Term> GetList()
        {
            string sql = "SELECT * FROM li_terms ORDER BY `order`";

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);

            List<Term> list = CacheHelper.Get<List<Term>>(TERMLIST_KEY);
            if (list == null)
            {
                list = context.ExecuteList<Term>(LoadModle);
                CacheHelper.Set(TERMLIST_KEY, list, 86400);
            }

            return list;
        }

        public TResult<Term> Get(int id)
        {
            string sql = "SELECT * FROM li_terms WHERE id=@id";
            string key = string.Format(TERM_KEY, id);

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@id", MySqlDbType.Int32, 4, id);

            TResult<Term> result = CacheHelper.Get<TResult<Term>>(key);
            if (result == null)
            {
                result = context.ExecuteTResult<Term>(LoadModle);
                CacheHelper.Set(key, result, 86400);
            }

            return result;
        }

        public TResult<Term> Get(string slug)
        { 
            string sql = "SELECT * FROM li_terms WHERE slug=@slug";
            string key = string.Format(TERM_KEY, slug);

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@slug", MySqlDbType.VarChar, 16, slug);

            TResult<Term> result = CacheHelper.Get<TResult<Term>>(key);
            if (result == null)
            {
                result = context.ExecuteTResult<Term>(LoadModle);
                CacheHelper.Set(key, result, 86400);
            }
            return result; 
        }

        public Result Add(Term model)
        {
            StringBuilder s = new StringBuilder();
            s.Append("INSERT INTO `li_terms`(`name`,`slug`,`group`,`order`) ");
            s.Append("VALUES (@name,@slug,@group,@order)");

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, s.ToString());

            context.AddParameter("@name", MySqlDbType.VarChar, 16, model.name);
            context.AddParameter("@slug", MySqlDbType.VarChar, 16, model.slug);
            context.AddParameter("@group", MySqlDbType.Int32, 4, model.group);
            context.AddParameter("@order", MySqlDbType.Byte, -1, model.order);

            Result result = context.ExecuteResult();

            return result;
        }

        public Result Modify(Term model)
        {
            StringBuilder s = new StringBuilder();
            s.Append("UPDATE li_terms SET `name`=@name,`slug`=@slug,`group`=@group,`order`=@order ");
            s.Append("WHERE `id`=@id");

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, s.ToString());
            context.AddParameter("@id", MySqlDbType.Int32, 4, model.id);
            context.AddParameter("@name", MySqlDbType.VarChar, 16, model.name);
            context.AddParameter("@slug", MySqlDbType.VarChar, 16, model.slug);
            context.AddParameter("@group", MySqlDbType.Int32, 4, model.group);
            context.AddParameter("@order", MySqlDbType.Byte, -1, model.order);

            Result result = context.ExecuteResult();

            return result;
        }

        #region ------Load Model------
        /// <summary>
        /// 加载下载基本信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private Term LoadModle(IDataReader dr)
        {
            Term model = new Term();
            model.id = dr.GetInt32("id");
            model.name = dr.GetString("name");
            model.slug = dr.GetString("slug");
            model.group = dr.GetInt32("group");
            return model;
        }
        #endregion
    }
}
