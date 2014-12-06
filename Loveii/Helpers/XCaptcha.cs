using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Drawing.Imaging;

namespace Loveii
{
    public class XCaptcha
    {
        private static Font _textStyleFont = new Font("Verdana", 30f, FontStyle.Regular);
        private static Brush _textStyleBrush = Brushes.Red;

        private static int _canvasWidth = 120; //0xaf;
        private static int _canvasHeight = 50;
        private static Brush _canvasBrush = new SolidBrush(Color.White);

        /// <summary>
        /// 生成验证码
        /// </summary>
        public static byte[] Create(string text)
        {
            byte[] returnByte = null;
            using (Bitmap bmp = new Bitmap(_canvasWidth, _canvasHeight))
            {
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    CanvasCreate(g, _canvasWidth, _canvasHeight);
                    NoiseCreate(g, _canvasWidth, _canvasHeight, _textStyleBrush);

                    GraphicsPath path = new GraphicsPath();

                    PointF startPoint = TextStyleCalcStartPoint(g, _canvasWidth, text, _textStyleFont);

                    g.PageUnit = GraphicsUnit.Point;
                    path.AddString(text, _textStyleFont.FontFamily, (int)_textStyleFont.Style, _textStyleFont.Size, startPoint, StringFormat.GenericDefault);

                    path = DistortCreate(path, _canvasWidth, _canvasHeight);

                    g.FillPath(_textStyleBrush, path);
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.Flush();
                    MemoryStream stream = new MemoryStream();
                    bmp.Save(stream, ImageFormat.Gif);
                    returnByte = stream.ToArray();
                }
            }
            return returnByte;
        }

        public static PointF TextStyleCalcStartPoint(Graphics graphics, int canvasWidth, string text, Font textStyleFont)
        {
            SizeF fontSize = graphics.MeasureString(text, textStyleFont);
            float x = (canvasWidth - fontSize.Width) / 2.5f;
            int y = 0;
            return new PointF(x, (float)y);
        }

        public static void CanvasCreate(Graphics graphics, int width, int height)
        {
            Rectangle rect = new Rectangle(0, 0, width, height);
            graphics.FillRectangle(new SolidBrush(Color.White), rect);
        }

        public static GraphicsPath DistortCreate(GraphicsPath path, int width, int height)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            Random rnd = new Random();
            double amp = (2 * height) / 0x55;
            double size = (rnd.NextDouble() * (width / 4)) + (width / 8);
            PointF[] pn = new PointF[path.PointCount];
            byte[] pt = new byte[path.PointCount];
            GraphicsPath np2 = new GraphicsPath();
            GraphicsPathIterator iter = new GraphicsPathIterator(path);
            for (int i = 0; i < iter.SubpathCount; i++)
            {
                bool closed;
                GraphicsPath sp = new GraphicsPath();
                iter.NextSubpath(sp, out closed);
                Matrix m = new Matrix();
                m.RotateAt(Convert.ToSingle((double)((rnd.NextDouble() * 30.0) - 15.0)), sp.PathPoints[0]);
                m.Translate((float)(-1 * i), 0f);
                sp.Transform(m);
                np2.AddPath(sp, true);
            }
            for (int i = 0; i < np2.PointCount; i++)
            {
                pn[i] = Wave(np2.PathPoints[i], amp, size);
                pt[i] = np2.PathTypes[i];
            }
            return new GraphicsPath(pn, pt);
        }

        private static PointF Wave(PointF p, double amp, double size)
        {
            p.Y = Convert.ToSingle((double)(Math.Sin(((double)p.X) / size) * amp)) + p.Y;
            p.X = Convert.ToSingle((double)(Math.Sin(((double)p.X) / size) * amp)) + p.X;
            return p;
        }

        public static void NoiseCreate(Graphics graphics, int width, int height,Brush brush)
        {
            Random ran = new Random();
            PointF[] curvePs = new PointF[10];
            for (int u = 0; u < 5; u++)
            {
                curvePs[u].X = u * (width / 5);
                curvePs[u].Y = height / 2;
            }
            for (int u = 5; u < 10; u++)
            {
                curvePs[u].X = (u - 5) * (width / 5);
                curvePs[u].Y = ran.Next(height);
            }
            GraphicsPath path = new GraphicsPath();
            path.AddLines(curvePs);
            graphics.DrawPath(new Pen(brush), path);
        }


    }

}
