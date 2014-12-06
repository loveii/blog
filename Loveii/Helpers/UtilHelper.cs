using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Text.RegularExpressions;

namespace Loveii.Helpers
{
    public static class UtilHelper
    {
        private static string Regex_UserName = @"^[A-Za-z0-9]{5,16}$";
        private static string Regex_Password = @"^.{6,16}$";//@"^[A-Za-z0-9]{6,16}$";
        private static string Regex_Email = @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$";
        private static string Regex_Mobile = @"^1[3458]\d{9}$";
        private static string Regex_Phone = @"^\d{7,15}$";//@"^([\d]{3,5}-)?[\d]{7,8}$";
        private static string Regex_ZIP = @"^\d{6}$";//@"^[1-9]\d{5}(?!\d)$";
        private static string Regex_RealName = @"^[\u4E00-\u9FA5]{2,8}$";
        private static string Regex_Chinese = @"^[\u4e00-\u9fa5]+$";
        private static string Regex_NickName = @"[^\u4E00-\u9FA5|^a-zA-Z|^0-9]";
        private static string Regex_ip = @"((?:(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(?:25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d))))";


        public static string Regex_ReplaceUserName = @"[A-Za-z0-9]";
        /// <summary>
        /// 验证用户名规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsUserName(this string input)
        {

            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
            {
                return isMatch;
            }
            if (Regex.IsMatch(input, @"^[a-zA-Z]") && Regex.IsMatch(input, Regex_UserName))
            {
                isMatch = true;
            }
            return isMatch;
        }

        /// <summary>
        /// 验证昵称
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNickName(this string input)
        {
            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
                return isMatch;
            isMatch = Regex.IsMatch(input, Regex_NickName) ? false : true;
            return isMatch;
        }

        /// <summary>
        /// 验证密码规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPassword(this string input)
        {
            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
            {
                return isMatch;
            }
            if (!Regex.IsMatch(input, @"^(.)\1+$") && Regex.IsMatch(input, Regex_Password))
            {
                isMatch = true;
            }

            return isMatch;
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string input)
        {
            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
                return isMatch;
            isMatch = Regex.IsMatch(input, Regex_Email);
            if (isMatch)
            {
                if (input.Length > 4)
                {
                    string suffix1 = input.Substring(input.Length - 4, 4).ToUpper();
                    string suffix2 = input.Substring(input.Length - 3, 3).ToUpper();
                    return (suffix1 == ".COM" || suffix1 == ".NET" || suffix1 == ".ORG" || suffix2 == ".CN");
                }
            }
            return isMatch;
        }

        /// <summary>
        /// 验证手机格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsMobile(this string input)
        {
            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
                return isMatch;
            isMatch = Regex.IsMatch(input, Regex_Mobile);
            return isMatch;
        }

        /// <summary>
        /// 验证固话格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsPhone(this string input)
        {
            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
                return isMatch;
            isMatch = Regex.IsMatch(input, Regex_Phone);
            return isMatch;
        }

        /// <summary>
        /// 验证邮编格式
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsZip(this string input)
        {
            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
                return isMatch;
            isMatch = Regex.IsMatch(input, Regex_ZIP);
            return isMatch;
        }

        /// <summary>
        /// 验证姓名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsRealName(this string input)
        {
            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
                return isMatch;
            isMatch = Regex.IsMatch(input, Regex_RealName);
            return isMatch;
        }

        /// <summary>
        /// 判断是否为中文
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsChinese(this string input)
        {
            bool isMatch = false;
            if (string.IsNullOrEmpty(input))
                return isMatch;
            isMatch = Regex.IsMatch(input, Regex_Chinese);
            return isMatch;
        }

        /// <summary>
        /// 转换整型
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ParseInt(string str, int defaultValue)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(str, @"^-?\d+$"))
                return int.Parse(str);

            return defaultValue;
        }

        /// <summary>
        /// 取随机产生的字符
        /// </summary>
        /// <param name="codeBit">字符位数</param>
        public static string GetRandom(int codeBit)
        {
            string Vchar = "1,2,3,4,5,6,7,8,9,0";
            string[] VcArray = Vchar.Split(',');
            string str = ""; //返回字符串
            int temp = -1;//记录上次随机数值，尽量避免生产几个一样的随机数
            //采用一个简单的算法以保证生成随机数的不同
            Random rand = new Random();
            for (int i = 1; i < codeBit + 1; i++)
            {
                if (temp != -1)
                {
                    rand = new Random(i * temp * unchecked((int)DateTime.Now.Ticks));
                }
                int t = rand.Next(10);
                if (temp != -1 && temp == t)
                {
                    return GetRandom(codeBit);
                }
                temp = t;
                str += VcArray[t];
            }

            return str;
        }

        #region ----- GET IP -----

        /// <summary>
        /// 获取客户端真实IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string stringIP = "";

            try
            {
                HttpRequest Request = System.Web.HttpContext.Current.Request;

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_Cdn_Src_Ip"]))
                {
                    stringIP = Request.ServerVariables["HTTP_Cdn_Src_Ip"];
                }

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    MatchCollection mc = Regex.Matches(Request.ServerVariables["HTTP_X_FORWARDED_FOR"], Regex_ip);
                    if (mc.Count > 0)
                    {
                        stringIP = mc[0].ToString();
                    }
                }

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_CLIENT_IP"]))
                {
                    stringIP = Request.ServerVariables["HTTP_CLIENT_IP"];
                }

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.ServerVariables["REMOTE_ADDR"]))
                {
                    stringIP = Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.UserHostAddress))
                {
                    stringIP = Request.UserHostAddress;
                }

                if (stringIP == "::1")
                {
                    stringIP = "127.0.0.1";
                }

            }
            catch
            {
                stringIP = "";
            }

            return string.IsNullOrEmpty(stringIP) ? "0.0.0.0" : stringIP;
        }

        /// <summary>
        /// 获取真实IP
        /// </summary>
        public static long GetLongIP()
        {
            string stringIP = "";

            try
            {
                HttpRequest Request = System.Web.HttpContext.Current.Request;

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_Cdn_Src_Ip"]))
                {
                    stringIP = Request.ServerVariables["HTTP_Cdn_Src_Ip"];
                }

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"]))
                {
                    MatchCollection mc = Regex.Matches(Request.ServerVariables["HTTP_X_FORWARDED_FOR"], Regex_ip);
                    if (mc.Count > 0)
                    {
                        stringIP = mc[0].ToString();
                    }
                }

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.ServerVariables["HTTP_CLIENT_IP"]))
                {
                    stringIP = Request.ServerVariables["HTTP_CLIENT_IP"];
                }

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.ServerVariables["REMOTE_ADDR"]))
                {
                    stringIP = Request.ServerVariables["REMOTE_ADDR"];
                }

                if (string.IsNullOrEmpty(stringIP) && !string.IsNullOrEmpty(Request.UserHostAddress))
                {
                    stringIP = Request.UserHostAddress;
                }

                if (stringIP == "::1")
                {
                    stringIP = "127.0.0.1";
                }

                return string.IsNullOrEmpty(stringIP) ? 0 : ConvertIP2Long(stringIP);
            }
            catch
            {
                return ConvertIP2Long("0.0.0.0");
            }
        }

        /// <summary>
        /// IP转换为长整型
        /// </summary>
        public static long ConvertIP2Long(string ip)
        {

            string[] ipSplit = ip.Split('.');
            if (ipSplit.Length != 4)
                return 0;

            int tmpIP = -1;
            for (int i = 0; i < ipSplit.Length; i++)
            {
                tmpIP = ParseInt(ipSplit[i], -1);
                if (tmpIP < 0 || tmpIP > 255)
                {
                    return 0;
                }
            }

            return (Int64.Parse(ipSplit[0]) * 256 * 256 * 256) + (Int64.Parse(ipSplit[1]) * 256 * 256) + (Int64.Parse(ipSplit[2]) * 256) + (Int64.Parse(ipSplit[3])) * 1;
        }

        /// <summary>
        /// 长整型转换为IP
        /// </summary>
        public static string ConvertLong2IP(long ip)
        {
            if (ip > 4294967295 || ip < 0)
                return "";

            return ((ip & 0xFF000000) / 16777216).ToString() + "." + ((ip & 0x00FF0000) / 65536).ToString() + "." + ((ip & 0x0000FF00) / 256).ToString() + "." + (ip & 0x000000FF).ToString();
        }

        /// <summary>
        /// IP转换为整型
        /// </summary>
        public static int ConvertIP2Int(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return 0;

            byte[] b = System.Net.IPAddress.Parse(ip).GetAddressBytes();

            return BitConverter.ToInt32(b, 0);
        }

        #endregion

        #region ---------身份证合法性验证----------

        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="Id">身份证号码</param>
        /// <returns>true:通过 false:失败</returns>
        public static bool CheckIDCard(string Id)
        {
            bool check = false;

            if (string.IsNullOrEmpty(Id) || (Id.Length != 15 && Id.Length != 18))
                return check;

            if (Id.Length == 18)
                check = CheckIDCard18(Id);
            else if (Id.Length == 15)
                check = CheckIDCard15(Id);

            return check;
        }

        private static bool CheckIDCard18(string Id)
        {

            long n = 0;

            if (long.TryParse(Id.Remove(17), out n) == false || n < Math.Pow(10, 16) || long.TryParse(Id.Replace('x', '0').Replace('X', '0'), out n) == false)
            {
                return false;//数字验证
            }

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
            {
                return false;//省份验证
            }

            string birth = Id.Substring(6, 8).Insert(6, "-").Insert(4, "-");

            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)
            {
                return false;//生日验证
            }

            //if ((DateTime.Now - time).Days < 365 * 16)
            //{
            //    return false;//小于16岁
            //}

            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');

            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');

            char[] Ai = Id.Remove(17).ToCharArray();

            int sum = 0;

            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }

            int y = -1;

            Math.DivRem(sum, 11, out y);

            if (arrVarifyCode[y] != Id.Substring(17, 1).ToLower())
                return false;//校验码验证

            return true;//符合GB11643-1999标准

        }

        private static bool CheckIDCard15(string Id)
        {

            long n = 0;

            if (long.TryParse(Id, out n) == false || n < Math.Pow(10, 14))
                return false;//数字验证

            string address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";

            if (address.IndexOf(Id.Remove(2)) == -1)
                return false;//省份验证

            string birth = Id.Substring(6, 6).Insert(4, "-").Insert(2, "-");

            DateTime time = new DateTime();

            if (DateTime.TryParse(birth, out time) == false)
                return false;//生日验证

            return true;//符合15位身份证标准
        }

        #endregion
    }
}
