using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Loveii
{
    /// <summary>
    /// SQLHelper的封装类，存储过程调用上下文
    /// </summary>
    public class ProcedureContext
    {
        /// <summary>
        /// 加载实体委托
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public delegate T LoadModel<T>(SqlDataReader dr);

        /// <summary>
        /// 存储过程名称
        /// </summary>
        public string ProcedureName { get; set; }

        /// <summary>
        /// 参数列表
        /// </summary>
        public List<SqlParameter> Parameters { get; set; }

        /// <summary>
        /// 数据库连接
        /// </summary>
        public SqlConnection Connection { get; set; }

        /// <summary>
        /// 默认构造
        /// </summary>
        public ProcedureContext()
        {
            this.Parameters = new List<SqlParameter>();
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="procedureName">存储过程名称</param>
        public ProcedureContext(SqlConnection connection, string procedureName)
            : this()
        {
            this.Connection = connection;
            this.ProcedureName = procedureName;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="connection">数据库连接</param>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        public ProcedureContext(SqlConnection connection, string procedureName, params SqlParameter[] parameters)
            : this(connection, procedureName)
        {
            foreach (SqlParameter item in parameters)
            {
                Parameters.Add(item);
            }
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="key">连接Key</param>
        /// <param name="procedureName">存储过程名称</param>
        public ProcedureContext(string key, string procedureName)
            : this()
        {
            this.Connection = GetConnectionByKey(key);
            this.ProcedureName = procedureName;
        }

        /// <summary>
        /// 有参构造
        /// </summary>
        /// <param name="key">连接Key</param>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="parameters">参数</param>
        public ProcedureContext(string key, string procedureName, params SqlParameter[] parameters)
            : this(key, procedureName)
        {
            foreach (SqlParameter item in parameters)
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
        public SqlConnection GetConnectionByKey(string key)
        {
            TResult<SqlConnection> result = DBConnection.GetSqlConnection(key);
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
        public SqlParameter[] GetParametersArray()
        {
            SqlParameter[] parasArray = new SqlParameter[Parameters.Count];
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
            Parameters.Add(new SqlParameter(parameterName, GetValue(value)));
        }

        /// <summary>
        /// 添加参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <param name="direction">参数转向类型</param>
        public void AddParameter(string parameterName, ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(parameterName, null);
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
        public void AddParameter(string parameterName, SqlDbType dbType, int size, object value)
        {
            SqlParameter param = new SqlParameter(parameterName, dbType, size);
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
        public void AddParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction)
        {
            SqlParameter param = new SqlParameter(parameterName, dbType, size);
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
        public void AddParameter(string parameterName, SqlDbType dbType, int size, ParameterDirection direction, object value)
        {
            SqlParameter param = new SqlParameter(parameterName, dbType, size);
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
            foreach (SqlParameter item in Parameters)
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
            ListResult<T> listResult = new ListResult<T>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(Connection, ProcedureName, GetParametersArray()))
            {
                while (dr.Read())
                {
                    listResult.Item.Add(loadModel(dr));
                }
                dr.Close();
                Connection.Close();
            }
            listResult.Successed = Convert.ToInt32(Parameters[returnValueIndex].Value) == 0 ? true : false;
            listResult.Message = Parameters[chvMsgIndex].Value.ToString();

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
        public TResult<T> ExecuteTResult<T>(int returnValueIndex, int chvMsgIndex, LoadModel<T> loadModel)
        {
            TResult<T> tResult = new TResult<T>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(Connection, ProcedureName, GetParametersArray()))
            {
                if (dr.Read())
                {
                    tResult.Item = loadModel(dr);
                }
                dr.Close();
                Connection.Close();
            }

            tResult.Successed = Convert.ToInt32(Parameters[returnValueIndex].Value) == 0 ? true : false;
            tResult.Message = Parameters[chvMsgIndex].Value.ToString();

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
        public PageResult<T> ExecutePageResult<T>(int pageIndex, int pageSize, int totalCountIndex, int returnValueIndex, int chvMsgIndex, LoadModel<T> loadModel)
        {
            PageResult<T> pageResult = new PageResult<T>();
            using (SqlDataReader dr = SQLHelper.ExecuteReader(Connection, ProcedureName, GetParametersArray()))
            {
                while (dr.Read())
                {
                    pageResult.Item.Add(loadModel(dr));
                }
                dr.Close();
                Connection.Close();
            }
            pageResult.PageIndex = pageIndex;
            pageResult.PageSize = pageSize;

            if (Parameters[totalCountIndex] != null && Parameters[totalCountIndex].Value != DBNull.Value)
            {
                pageResult.TotalCount = Convert.ToInt32(Parameters[totalCountIndex].Value);
            }
            if (Parameters[returnValueIndex] != null && Parameters[returnValueIndex].Value != DBNull.Value)
            {
                pageResult.Successed = Convert.ToInt32(Parameters[returnValueIndex].Value) == 0 ? true : false;
            }
            pageResult.Message = Parameters[chvMsgIndex].Value.ToString();

            return pageResult;
        }

        /// <summary>
        /// 获取分页列表
        /// 该方法默认为:
        /// PageIndex = 1,
        /// PageSize = 10,
        /// TotalCount = 0
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="returnValueIndex">返回值索引</param>
        /// <param name="chvMsgIndex">返回消息索引</param>
        /// <param name="loadModel">返回消息索引</param>
        /// <returns></returns>
        public PageResult<T> ExecutePageResult<T>(int returnValueIndex, int chvMsgIndex, LoadModel<T> loadModel)
        {
            return ExecutePageResult<T>(1, 10, 0, returnValueIndex, chvMsgIndex, loadModel);
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
            using (SqlDataReader dr = SQLHelper.ExecuteReader(Connection, ProcedureName, GetParametersArray()))
            {
                while (dr.Read())
                {
                    result.Add(loadModel(dr));
                }
                dr.Close();
                Connection.Close();
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
            using (SqlDataReader dr = SQLHelper.ExecuteReader(Connection, ProcedureName, GetParametersArray()))
            {
                if (dr.Read())
                {
                    t = loadModel(dr);
                }
                dr.Close();
                Connection.Close();
            }
            return t;
        }

        /// <summary>
        /// 获取Result实体结果
        /// </summary>
        /// <param name="returnValueIndex">返回值索引</param>
        /// <param name="chvMsgIndex">消息索引</param>
        /// <returns></returns>
        public Result ExecuteResult(int returnValueIndex, int chvMsgIndex)
        {
            Result result = new Result();
            SQLHelper.ExecuteNonQuery(Connection, ProcedureName, GetParametersArray());

            result.Message = Parameters[chvMsgIndex].Value.ToString();
            result.Successed = Convert.ToInt32(Parameters[returnValueIndex].Value) == 0 ? true : false;

            return result;
        }

        /// <summary>
        /// 获取首行首列
        /// </summary>
        /// <returns></returns>
        public object ExecuteScalar()
        {
            return SQLHelper.ExecuteScalar(Connection, ProcedureName, GetParametersArray());
        }
    }
}
