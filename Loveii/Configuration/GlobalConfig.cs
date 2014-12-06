using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;

namespace Loveii
{
    /// <summary>
    /// 全局配置
    /// </summary>
    [Serializable()]
    public class GlobalConfig
    {
        [ThreadStatic]
        private static object _lockHelper = new object();
        private static GlobalConfig _globalConfig;
        private static string _fileName = AppDomain.CurrentDomain.BaseDirectory + "Global.config";


        private GlobalConfig()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public static GlobalConfig Instance
        {
            get
            {
                if (_globalConfig == null)
                {
                    if (_lockHelper == null)
                        _lockHelper = new object();

                    lock (_lockHelper)
                    {
                        if (_globalConfig == null)
                        {
                            _globalConfig = (GlobalConfig)LoadXml(_fileName, typeof(GlobalConfig));
                        }
                    }
                }

                return _globalConfig;
            }
        }

        /// <summary>
        /// 网站主域
        /// </summary>
        public string Domain { get; set; }
 
        /// <summary>
        /// 加载xml
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object LoadXml(string filePath, Type type)
        {
            if (!System.IO.File.Exists(filePath))
                return null;

            StreamReader reader = null;
            try
            {
                using (reader = new StreamReader(filePath))
                {
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);
                    object obj = xs.Deserialize(reader);
                    return obj;
                }
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }
    }
}
