using System;
using System.Data;
using System.Data.SqlClient;

namespace Loveii
{
    /// <summary>
    /// sql处理辅处类
    /// </summary>
    public class SQLHelper
    {
        #region ExecuteResult

        /// <summary>
        /// 返回Result实体结果   
        /// </summary>
        /// <param name="conn">SqlConnection</param>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="returnValue">返回状态索引id</param>
        /// <param name="chvMsg">返回消息索引id</param>
        /// <param name="commandParameters">存储过程参数(需求含返回状态参数和返回消息参数)</param>
        /// <returns>状态类型结果</returns>
        public static Result ExecuteResult(SqlConnection conn, string procedureName, int returnValue, int chvMsg, params SqlParameter[] commandParameters)
        {
            ExecuteNonQuery(conn, CommandType.StoredProcedure, procedureName, commandParameters);

            Result result = new Result();
            result.Successed = Convert.ToInt32(commandParameters[returnValue].Value) == 0 ? true : false;
            result.Message = commandParameters[chvMsg].Value.ToString();

            return result;
        }

        #endregion

        #region ExecuteNonQuery

        /// <summary>
        /// 执行指定的SQL
        /// </summary>
        /// <param name="conn">SqlConnection</param>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>返回命令影响的行数</returns>
        public static int ExecuteNonQuery(SqlConnection conn, string procedureName, params SqlParameter[] commandParameters)
        {
            return ExecuteNonQuery(conn, CommandType.StoredProcedure, procedureName, commandParameters);
        }

        /// <summary>
        /// 执行指定的SQL
        /// </summary>
        /// <param name="connection">SqlConnection</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">执行文本</param>
        /// <param name="commandParameters">sql参数</param>
        /// <returns>返回命令影响的行数</returns>
        public static int ExecuteNonQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (connection.State != ConnectionState.Closed)
                    connection.Close();
            }
        }

        /// <summary>
        /// 执行指定命令
        /// </summary>
        /// <param name="trans">sql事务</param>
        /// <param name="cmdType">执行类型</param>
        /// <param name="cmdText">执行文本</param>
        /// <param name="commandParameters">sql参数</param>
        /// <returns>返回命令影响的行数</returns>
        public static int ExecuteNonQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
                int val = cmd.ExecuteNonQuery();
                cmd.Parameters.Clear();
                return val;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (trans.Connection.State != ConnectionState.Closed)
                    trans.Connection.Close();
            }
        }
        #endregion

        #region ExecuteReader

        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="procedureName">存储过程</param>
        /// <param name="commandParameters">存储过程参数</param>
        /// <returns>返回SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(SqlConnection conn, string procedureName, params SqlParameter[] commandParameters)
        {
            return ExecuteReader(conn,CommandType.StoredProcedure, procedureName, commandParameters);
        }

        /// <summary>
        /// 执行ExecuteReader
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>返回SqlDataReader</returns>
        public static SqlDataReader ExecuteReader(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

                return rdr;
            }
            catch (Exception ex)
            {
                if (conn != null && conn.State != ConnectionState.Closed) conn.Close();

                throw ex;
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="procedureName">存储过程名称</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>返回执行结果object</returns>
        public static object ExecuteScalar(SqlConnection conn, string procedureName, params SqlParameter[] commandParameters)
        {
            return ExecuteScalar(conn, CommandType.StoredProcedure, procedureName, commandParameters);
        }

        /// <summary>
        /// 执行ExecuteScalar
        /// </summary>
        /// <param name="conn">数据库连接</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令文本</param>
        /// <param name="commandParameters">命令参数</param>
        /// <returns>返回执行结果object</returns>
        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            SqlCommand cmd = new SqlCommand();
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                object val = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return val;
            }
            catch
            {
                throw;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
            }
        }

        #endregion

        #region PrepareCommand

        /// <summary>
        /// 预处理用户提供的命令,数据库连接/事务/命令类型/参数
        /// </summary>
        /// <param name="cmd">数据库命令</param>
        /// <param name="conn">数据库连接</param>
        /// <param name="trans">数据库事务</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">命令内容</param>
        /// <param name="cmdParms">命令参数</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }
        #endregion

        #region DataConvert

        /// <summary>
        /// Boolean
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static bool GetBoolean(IDataReader dr, string columns)
        {
            return dr.GetBoolean(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// Byte
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static byte GetByte(IDataReader dr, string columns)
        {
            return dr.GetByte(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// DateTime
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(IDataReader dr, string columns)
        {
            return dr.GetDateTime(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// Decimal
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static decimal GetDecimal(IDataReader dr, string columns)
        {
            return dr.GetDecimal(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// Double
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static double GetDouble(IDataReader dr, string columns)
        {
            return dr.GetDouble(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// Float
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static float GetFloat(IDataReader dr, string columns)
        {
            return dr.GetFloat(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// Guid
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Guid GetGuid(IDataReader dr, string columns)
        {
            return dr.GetGuid(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// Int16
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static short GetInt16(IDataReader dr, string columns)
        {
            return dr.GetInt16(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// Int32
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static int GetInt32(IDataReader dr, string columns)
        {
            int i = dr.GetOrdinal(columns);
            return dr.GetInt32(i);
        }

        /// <summary>
        /// Int64
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static long GetInt64(IDataReader dr, string columns)
        {
            return dr.GetInt64(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// String
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static string GetString(IDataReader dr, string columns)
        {
            return dr.GetString(dr.GetOrdinal(columns));
        }

        /// <summary>
        /// Char
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static char GetChar(IDataReader dr,string columns)
        {
            return dr.GetChar(dr.GetOrdinal(columns));
        }

        #endregion
    }

}
