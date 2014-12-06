using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
using System.Data.SqlClient;
using System.Data.Common;

namespace System.Data
{
    /// <summary>
    /// IDataReader扩展
    /// </summary>
    public static class IDataReaderExt
    {
        /// <summary>
        /// 通用Get
        /// </summary>
        public static T Get<T>(this IDataReader dr, string name)
        {
            object obj = dr[name];
            if (obj == DBNull.Value)
                return default(T);

            return (T)obj;
        }

        /// <summary>
        /// Boolean
        /// </summary>
        public static bool GetBoolean(this IDataReader dr, string columns)
        {
            return Get<bool>(dr,columns);
        }

        /// <summary>
        /// Byte
        /// </summary>
        public static byte GetByte(this IDataReader dr, string columns)
        {
            return Get<byte>(dr, columns); 
        }

        /// <summary>
        /// Bytes
        /// </summary>
        public static byte[] GetBytes(this IDataReader dr, string columns)
        {
            return Get<byte[]>(dr, columns);
        }

        /// <summary>
        /// Bytes
        /// </summary>
        public static byte[] GetBytes(this IDataReader dr, string columns,int length)
        {
            byte[] buffer = new byte[length];

            int i = dr.GetOrdinal(columns);

            dr.GetBytes(i, 0, buffer, 0, length);

            return buffer;
        }

        /// <summary>
        /// DateTime
        /// </summary>
        public static DateTime GetDateTime(this IDataReader dr, string columns)
        {
            return Get<DateTime>(dr, columns); 
        }

        /// <summary>
        /// Decimal
        /// </summary>
        public static decimal GetDecimal(this IDataReader dr, string columns)
        {
            return Get<decimal>(dr, columns); 
        }

        /// <summary>
        /// Double
        /// </summary>
        public static double GetDouble(this IDataReader dr, string columns)
        {
            return Get<double>(dr, columns); 
        }

        /// <summary>
        /// Float
        /// </summary>
        public static float GetFloat(this IDataReader dr, string columns)
        {
            return Get<float>(dr, columns); 
        }

        /// <summary>
        /// Guid
        /// </summary>
        public static Guid GetGuid(this IDataReader dr, string columns)
        {
            return Get<Guid>(dr, columns); 
        }

        /// <summary>
        /// Int16
        /// </summary>
        public static short GetInt16(this IDataReader dr, string columns)
        {
            return Get<short>(dr, columns); 
        }

        /// <summary>
        /// Int32
        /// </summary>
        public static int GetInt32(this IDataReader dr, string columns)
        {
            return Get<int>(dr, columns); 
        }

        /// <summary>
        /// Int64
        /// </summary>
        public static long GetInt64(this IDataReader dr, string columns)
        {
            return Get<long>(dr, columns); 
        }

        /// <summary>
        /// String
        /// </summary>
        public static string GetString(this IDataReader dr, string columns)
        {
            return Get<string>(dr, columns); 
        }

        /// <summary>
        /// Char
        /// </summary>
        public static char GetChar(this IDataReader dr, string columns)
        {
            return Get<char>(dr, columns); 
        }
    }
}
