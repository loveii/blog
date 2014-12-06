using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;

namespace Loveii
{
    /// <summary>
    /// 安全验证码帮助类
    /// </summary>
    public class CaptchaHelper
    {
        /// <summary>
        /// 设置Session验证码
        /// </summary>
        /// <param name="randomText"></param>
        public static void Set(string randomText)
        {
            HttpContext.Current.Session.Add("RandomText", randomText);
        }

        /// <summary>
        /// 获取验证码
        /// </summary>
        /// <returns></returns>
        private static string GetCaptcha()
        {
            if (HttpContext.Current.Session["RandomText"] == null)
                return string.Empty;

            return HttpContext.Current.Session["RandomText"].ToString();
        }

        /// <summary>
        /// 比较验证码是否正确
        /// </summary>
        /// <param name="randomText"></param>
        /// <returns></returns>
        public static bool Compare(string randomText)
        {
            if (string.IsNullOrEmpty(randomText))
                return false;

            string s = GetCaptcha();
            if (s.ToLower() != randomText.ToLower())
                return false;

            return true;
        }

        /// <summary>
        /// 新版占CPU高 获取Byte[]类型验证码
        /// </summary>
        /// <returns>Byte[]</returns>
        public static byte[] GetByteCode()
        {
            string str = GetRandom(4);
            Set(str);

            return XCaptcha.Create(str);
        }

        /// <summary>
        /// 获取Byte[]类型验证码
        /// </summary>
        /// <param name="randomText"></param>
        /// <returns></returns>
        public static byte[] GetByte()
        {
            string randomText = GetRandom(4);

            Bitmap Img = null;
            Graphics g = null;
            MemoryStream ms = null;
            Random random = new Random();
            int gheight = randomText.Length * 25;
            Img = new Bitmap(gheight, 40);
            g = Graphics.FromImage(Img);
            Font f = new Font("Arial", 22, FontStyle.Bold);

            g.Clear(GetByRandColor(200, 255));//设定背景色
            Pen blackPen = new Pen(Color.Ivory, 3);
            for (int i = 0; i < 155; i++)// 随机产生干扰线，使图象中的认证码不易被其它程序探测到
            {
                int x = random.Next(gheight);
                int y = random.Next(20);
                int xl = random.Next(1);
                int yl = random.Next(10);
                g.DrawLine(blackPen, x, y, x + xl, y + yl);
            }
            SolidBrush s = new SolidBrush(Color.SandyBrown);
            g.DrawString(randomText, f, s, 1, 3);
            ms = new MemoryStream();
            Img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            g.Dispose();
            Img.Dispose();

            Set(randomText);

            return ms.ToArray();
        }

        /// <summary>
        /// 给定范围获得随机颜色
        /// </summary>
        /// <param name="fc"></param>
        /// <param name="bc"></param>
        /// <returns></returns>
        private static Color GetByRandColor(int fc, int bc)
        {
            Random random = new Random();

            if (fc > 255) fc = 255;
            if (bc > 255) bc = 255;
            //if(ac>255) ac=255;
            int r = fc + random.Next(bc - fc);
            int g = fc + random.Next(bc - fc);
            int b = fc + random.Next(bc - bc);
            Color rs = Color.FromArgb(r, g, b);
            return rs;
        }

        /// <summary>
        /// 取随机产生的认证码
        /// </summary>
        /// <param name="codeBit">验证码位数</param>
        /// <returns></returns>
        public static string GetRandom(int codeBit)
        {
            string Vchar = "2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,j,k,m,n,p,q,r,s,t,u,w,x,y,z,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,V,M,X,Y,Z";
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
                int t = rand.Next(35);
                if (temp != -1 && temp == t)
                {
                    return GetRandom(codeBit);
                }
                temp = t;
                str += VcArray[t];
            }

            return str;
        }
    }
}
