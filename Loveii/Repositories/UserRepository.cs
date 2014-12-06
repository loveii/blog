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
    public class UserRepository
    {
        public static readonly string USER_KEY = "/user/get/{0}";

        public TResult<User> Get(int id)
        {
            string sql = "SELECT * FROM li_users WHERE `id` = @id";

            string key = string.Format(USER_KEY, id);

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@id", MySqlDbType.Int32, 4, id);

            TResult<User> model = CacheHelper.Get<TResult<User>>(key);
            if (model == null)
            {
                model = context.ExecuteTResult<User>(LoadModle);
                CacheHelper.Set(key, model, 86400);
            }
            return model;
        }

        public Result IsExist(string userName)
        {
            string sql = "SELECT * FROM li_users WHERE `userName` = @userName";

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@userName", MySqlDbType.VarChar, 16, userName);

            TResult<User> model = context.ExecuteTResult<User>(LoadModle);
            return model;
        }

        public TResult<User> Login(string userName, string password)
        {
            string sql = "SELECT * FROM li_users WHERE `userName` = @userName AND `password` = @password";

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, sql);
            context.AddParameter("@userName", MySqlDbType.VarChar, 16, userName);
            context.AddParameter("@password", MySqlDbType.VarChar, 40, password);

            TResult<User> model = context.ExecuteTResult<User>(LoadModle);

            return model;
        }

        public Result Add(User model)
        {
            StringBuilder s = new StringBuilder();
            s.Append("INSERT INTO li_users (`userName`,`password`,`niceName`,`email`,`url`,`status`) ");
            s.Append("VALUES (@userName,@password,@niceName,@email,@url,@status)");

            MySqlContext context = new MySqlContext(CreateRepository.Loveii_KEY, s.ToString());
            context.AddParameter("@userName", MySqlDbType.VarChar, 16, model.userName);
            context.AddParameter("@password", MySqlDbType.VarChar, 40, model.password);
            context.AddParameter("@niceName", MySqlDbType.VarChar, 16, model.niceName);
            context.AddParameter("@email", MySqlDbType.VarChar, 64, model.email);
            context.AddParameter("@url", MySqlDbType.VarChar, 128, model.url);
            context.AddParameter("@status", MySqlDbType.Byte, 1, model.status);

            Result result = context.ExecuteResult();

            return result;
        }

        #region ------Load Model------
        /// <summary>
        /// 加载下载基本信息
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private User LoadModle(IDataReader dr)
        {
            User model = new User();
            model.id = dr.GetInt32("id");
            model.email = dr.GetString("email");
            model.niceName = dr.GetString("niceName");
            model.password = dr.GetString("password");
            model.status = Convert.ToByte(dr["status"]);
            model.url = dr.GetString("url");
            model.userName = dr.GetString("userName");
            model.createTime = dr.GetDateTime("createTime");

            return model;
        }
        #endregion
    }
}
