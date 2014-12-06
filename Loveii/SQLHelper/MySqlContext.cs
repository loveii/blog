using System;
using System.Collections.Generic;
using System.Data;
using MySql.Data.MySqlClient;

namespace Loveii
{
    /// <summary>
    /// MySqlHelper的封装类，存储过程调用上下文
    /// </summary>
    public class MySqlContext
    {
        /// <summary>
        /// 加载实体委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public delegate T LoadModel<T>(MySqlDataReader dr);

        /// <summary>
        /// SQL语句
        /// </summary>
        public string SqlText { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<MySqlParameter> Parameters { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public MySqlConnection Connection { get; set; }

        /// <summary>
        /// 默认构造
        /// </summary>
        public MySqlContext()
        {
            this.Parameters = new List<MySqlParameter>();
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sqlText">存储过程名称</param>
        public MySqlContext(MySqlConnection connection, string sqlText)
            : this()
        {
            this.Connection = connection;
            this.SqlText = sqlText;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="sqlText">存储过程名称</param>
        /// <param name="parameters">参数</param>
        public MySqlContext(MySqlConnection connection, string sqlText, params MySqlParameter[] parameters)
            : this(connection, sqlText)
        {
            foreach (MySqlParameter item in parameters)
            {
                Parameters.Add(item);
            }
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="key">连接Key</param>
        /// <param name="sqlText">存储过程名称</param>
        public MySqlContext(string key, string sqlText)
            : this()
        {
            this.Connection = GetConnectionByKey(key);
            this.SqlText = sqlText;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="key">连接Key</param>
        /// <param name="sqlText">存储过程名称</param>
        /// <param name="parameters">参数</param>
        public MySqlContext(string key, string sqlText, params MySqlParameter[] parameters)
            : this(key, sqlText)
        {
            foreach (MySqlParameter item in parameters)
            {
                Parameters.Add(item);
            }
        }

        /// <summary>
        /// 获取值
        /// 防止传入null到SqlParameter
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private object GetValue(object value)
        {
            return value == null ? DBNull.Value : value;
        }

        /// <summary>
        /// 根据Key获取SqlConnection
        /// </summary>
        /// <param name="key">数据库Key</param>
        /// <returns>指定的SqlConnection</returns>
        public MySqlConnection GetConnectionByKey(string key)
        {
            TResult<MySqlConnection> result = DBConnection.GetMySqlConnection(key);
            if (result.Successed && result.Item != null)
            {
                return result.Item;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取数组类型的参数列表
        /// </summary>
        /// <returns></returns>
        public MySqlParameter[] GetParametersArray()
        {
            MySqlParameter[] parasArray = new MySqlParameter[Parameters.Count];
            for (int i = 0; i < Parameters.Count; i++)
            {
                if (Parameters[i].Value == null)
                    Parameters[i].Value = DBNull.Value;

                parasArray[i] = Parameters[i];
            }
            return parasArray;
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="value">值</param>
        public void AddParameter(string parameterName, object value)
        {
            Parameters.Add(new MySqlParameter(parameterName, GetValue(value)));
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="direction">参数转向类型</param>
        public void AddParameter(string parameterName, ParameterDirection direction)
        {
            MySqlParameter param = new MySqlParameter(parameterName, null);
            param.Direction = direction;
            Parameters.Add(param);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">参数数据库类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string parameterName, MySqlDbType dbType, int size, object value)
        {
            MySqlParameter param = new MySqlParameter(parameterName, dbType, size);
            param.Value = GetValue(value);
            Parameters.Add(param);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">参数数据库类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="direction">参数值</param>
        public void AddParameter(string parameterName, MySqlDbType dbType, int size, ParameterDirection direction)
        {
            MySqlParameter param = new MySqlParameter(parameterName, dbType, size);
            param.Direction = direction;
            Parameters.Add(param);
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="dbType">参数数据库类型</param>
        /// <param name="size">参数大小</param>
        /// <param name="direction">参数转向类型</param>
        /// <param name="value">参数值</param>
        public void AddParameter(string parameterName, MySqlDbType dbType, int size, ParameterDirection direction, object value)
        {
            MySqlParameter param = new MySqlParameter(parameterName, dbType, size);
            param.Direction = direction;
            param.Value = GetValue(value);
            Parameters.Add(param);
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parameterName">参数名</param>
        /// <returns></returns>
        public T GetParameter<T>(string parameterName)
        {
            Object obj = null;
            foreach (MySqlParameter item in Parameters)
            {
                if (parameterName.ToLower().Equals(item.ParameterName.ToLower()))
                {
                    obj = item.Value;
                    break;
                }
            }
            return (T)obj;
        }

        /// <summary>
        /// 获取泛型结果实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnValueIndex">返回值索引</param>
        /// <param name="chvMsgIndex">返回消息索引</param>
        /// <param name="loadModel"></param>
        /// <returns></returns>
        public ListResult<T> ExecuteListResult<T>(int returnValueIndex, int chvMsgIndex, LoadModel<T> loadModel)
        {
            ListResult<T> listResult = new ListResult<T>() { Successed = false, Message = "empty" };

            MySqlCommand myComm = new MySqlCommand(SqlText, Connection);
            try
            {
                myComm.Connection.Open();
                myComm.CommandType = CommandType.Text;
                myComm.Parameters.AddRange(GetParametersArray());
                MySqlDataReader dr = myComm.ExecuteReader();
                while (dr.Read())
                {
                    listResult.Item.Add(loadModel(dr));
                }
                if (dr.HasRows)
                {
                    listResult.Successed = true;
                    listResult.Message = "ok";
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                myComm.Connection.Close();
                myComm.Dispose();
            }

            return listResult;
        }

        /// <summary>
        /// 获取单个结果型体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="returnValueIndex">返回值索引</param>
        /// <param name="chvMsgIndex">返回消息索引</param>
        /// <param name="loadModel"></param>
        /// <returns></returns>
        public TResult<T> ExecuteTResult<T>(LoadModel<T> loadModel)
        {
            TResult<T> tResult = new TResult<T>() { Successed = false, Message = "empty" };

            MySqlCommand myComm = new MySqlCommand(SqlText, Connection);
            try
            {
                myComm.Connection.Open();
                myComm.CommandType = CommandType.Text;
                myComm.Parameters.AddRange(GetParametersArray());
                MySqlDataReader dr = myComm.ExecuteReader();
                while (dr.Read())
                {
                    tResult.Item = loadModel(dr);
                }
                if (dr.HasRows)
                {
                    tResult.Successed = true;
                    tResult.Message = "ok";
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                myComm.Connection.Close();
                myComm.Dispose();
            }

            return tResult;
        }

        /// <summary>
        /// 获取分页列表
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="totalCountIndex">分页总记录索引</param>
        /// <param name="returnValueIndex">返回值索引</param>
        /// <param name="chvMsgIndex">返回消息索引</param>
        /// <param name="loadModel">加载实体方法</param>
        /// <returns></returns>
        public PageResult<T> ExecutePageResult<T>(int pageIndex, int pageSize, int totalCount, LoadModel<T> loadModel)
        {
            PageResult<T> pageResult = new PageResult<T>() { Successed = false, Message = "empty" };

            MySqlCommand myComm = new MySqlCommand(SqlText, Connection);
            try
            {
                myComm.Connection.Open();
                myComm.CommandType = CommandType.Text;
                myComm.Parameters.AddRange(GetParametersArray());
                MySqlDataReader dr = myComm.ExecuteReader();
                while (dr.Read())
                {
                    pageResult.Item.Add(loadModel(dr));
                }
                if (dr.HasRows)
                {
                    pageResult.Successed = true;
                    pageResult.Message = "ok";
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                myComm.Connection.Close();
                myComm.Dispose();
            }


            pageResult.PageIndex = pageIndex;
            pageResult.PageSize = pageSize;
            pageResult.TotalCount = totalCount;

            return pageResult;
        }

        /// <summary>
        /// 获取查询结果集
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loadModel"></param>
        /// <returns></returns>
        public List<T> ExecuteList<T>(LoadModel<T> loadModel)
        {
            List<T> result = new List<T>();
            MySqlCommand myComm = new MySqlCommand(SqlText, Connection);
            try
            {
                myComm.Connection.Open();
                myComm.CommandType = CommandType.Text;
                myComm.Parameters.AddRange(GetParametersArray());
                MySqlDataReader dr = myComm.ExecuteReader();
                while (dr.Read())
                {
                    result.Add(loadModel(dr));
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                myComm.Connection.Close();
                myComm.Dispose();
            }

            return result;
        }

        /// <summary>
        /// 获取单个实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="loadModel"></param>
        /// <returns></returns>
        public T ExecuteT<T>(LoadModel<T> loadModel)
        {
            T t = Activator.CreateInstance<T>();
            MySqlCommand myComm = new MySqlCommand(SqlText, Connection);
            try
            {
                myComm.Connection.Open();
                myComm.CommandType = CommandType.Text;
                myComm.Parameters.AddRange(GetParametersArray());
                MySqlDataReader dr = myComm.ExecuteReader();
                while (dr.Read())
                {
                    t = loadModel(dr);
                }
                dr.Close();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                myComm.Connection.Close();
                myComm.Dispose();
            }

            return t;
        }

        /// <summary>
        /// 获取Result实体结果
        /// </summary>
        /// <returns></returns>
        public Result ExecuteResult()
        {
            Result result = new Result(false, "empty");

            MySqlCommand myComm = new MySqlCommand(SqlText, Connection);
            try
            {
                myComm.Connection.Open();
                myComm.CommandType = CommandType.Text;
                myComm.Parameters.AddRange(GetParametersArray());
                int r = myComm.ExecuteNonQuery();

                if (r > 0)
                {
                    result.Successed = true;
                    result.Message = "ok";
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                myComm.Connection.Close();
                myComm.Dispose();
            }



            return result;
        }

        /// <summary>
        /// 获取首行首列
        /// </summary>
        /// <returns></returns>
        public object ExecuteScalar()
        {
            object o = new object();
            MySqlCommand myComm = new MySqlCommand(SqlText, Connection);
            try
            {
                myComm.Connection.Open();
                myComm.CommandType = CommandType.Text;
                myComm.Parameters.AddRange(GetParametersArray());
                o = myComm.ExecuteScalar();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (Connection.State != ConnectionState.Closed)
                    Connection.Close();
                myComm.Connection.Close();
                myComm.Dispose();
            }

            return o;
        }
    }
}
