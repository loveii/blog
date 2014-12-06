using System;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;

namespace Loveii
{
    /// <summary>
    /// 获取数据库连接对象
    /// </summary>
    public sealed class DBConnection
    {
        /// <summary>
        /// 根据key,获取连接字符串
        /// </summary>
        /// <param name="key">连接key</param>
        /// <returns>ConnectionString</returns>
        public static string GetConnectionString(string key)
        {
            return GetResource.GetDataBase(key);
        }

        #region -----获取SqlConnection-----

        /// <summary>
        /// 根据key,获取连接对象
        /// </summary>
        /// <param name="key">连接key</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>结果对象类型(SqlConnection)</returns>
        public static TResult<SqlConnection> GetSqlConnection(string key)
        {
            TResult<SqlConnection> result = new TResult<SqlConnection>(false, "获取失败");

            string db = GetResource.GetDataBase(key);

            if (string.IsNullOrEmpty(db))
            {
                return result;
            } 
            try
            {
                result.Item = new SqlConnection(db);
                result.Successed = true;
                result.Message = "获取成功";
            }
            catch (Exception ex)
            {
                result.Successed = false;
                result.Message = string.Format("在打开key:{0}过程中出异常。信息如下：{1}", key, ex.Message);
                throw new Exception(result.Message);
            }

            return result;
        }

        #endregion

        #region -----获取MySqlConnection-----

        /// <summary>
        /// 根据key,获取连接对象
        /// </summary>
        /// <param name="key">连接key</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>结果对象类型(SqlConnection)</returns>
        public static TResult<MySqlConnection> GetMySqlConnection(string key)
        {
            TResult<MySqlConnection> result = new TResult<MySqlConnection>(false, "获取失败");

            string db = GetResource.GetDataBase(key);
  
            try
            { 
                result.Item = new MySqlConnection(db);
                result.Successed = true;
                result.Message = "获取成功";
            }
            catch (Exception ex)
            {
                result.Successed = false;
                result.Message = string.Format("在打开key:{0}过程中出异常。信息如下：{1}", key, ex.Message);
            }

            return result;
        }

        #endregion

        #region -----获取DbConnection-----

        /// <summary>
        /// 根据key,获取DbConnection
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataBaseType"></param>
        /// <returns></returns>
        public static DbConnection GetDbConnection(string key, DataBaseType dataBaseType)
        {
            return GetDbConnection(key, dataBaseType, 10);
        }

        /// <summary>
        /// 根据数据库类型(MySQL、SQLServer...)获取数据库连接
        /// </summary>
        /// <param name="key">连接key</param>
        /// <param name="timeOut">超时时间</param>
        /// <returns>DbConnection</returns>
        public static DbConnection GetDbConnection(string key, DataBaseType dataBaseType, int timeOut)
        {
            string db = GetResource.GetDataBase(key);
            try
            {
                if (dataBaseType == DataBaseType.SqlServer)
                {
                    DbConnection dbConn = new SqlConnection(db);//DbProviderFactories.GetFactory("System.Data.SqlClient").CreateConnection();
                    return dbConn;
                }

                if (dataBaseType == DataBaseType.MySql)
                {
                    DbConnection dbConn = new MySqlConnection(db);// DbProviderFactories.GetFactory("MySql.Data.MySqlClient").CreateConnection();
                    return dbConn;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;
        }

        #endregion

    }
}
