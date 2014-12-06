using System;
using Loveii.Utils;

namespace Loveii
{ 
    /// <summary>
    /// 数据库类型
    /// </summary>
    [Serializable]
    public enum DataBaseType
    {
        /// <summary>
        /// Microsoft SqlServer
        /// </summary>
        [EnumDescription("SqlServer数据库")]
        SqlServer = 1,

        /// <summary>
        /// MySql
        /// </summary>
        [EnumDescription("MySql数据库")]
        MySql = 2
    }

}
