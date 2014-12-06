using System;
using System.Collections.Generic; 

namespace Loveii
{
    /// <summary>
    /// 获取区服列表
    /// </summary>
    public class GetResource
    { 
        public static string GetDataBase(string key)
        {
            string sqlConn = System.Configuration.ConfigurationManager.AppSettings[key].ToString();
            return sqlConn;
        } 
 
    }
}
