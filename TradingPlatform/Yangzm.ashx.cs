using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace TradingPlatform.web
{
    /// <summary>
    /// Yangzm 的摘要说明
    /// </summary>
    public class Yangzm : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            //生产随机的 字母 数字

            //随机池
            string codes = "qwertyuiopasdfghjklzxcvbnm123456789123456789qwertyuiopasdfghjklzxcvbnm12345689qwesrtyujvfhmsdgbnx";

            //创建生成随机数对象
            Random r = new Random();

            int codeNum = 4;

            string retCode = "";

            for (int i = 0; i < codeNum; i++)
            {
                retCode += codes.Substring(r.Next(0, codes.Length - 1), 1);
            }

            //将随机数存入session中
            context.Session["retCode"] = retCode;

            //  建立 位图 
            Bitmap bit = new Bitmap(140, 40);

            Graphics g = Graphics.FromImage(bit);



            Color c = Color.FromArgb(r.Next(100, 250), r.Next(100, 200), r.Next(100, 120));

            g.Clear(c);

            //讲随机数  写到图里

            Brush[] Brushcolors = { Brushes.Yellow, Brushes.Purple, Brushes.Green, Brushes.GreenYellow, Brushes.Red, Brushes.Black, Brushes.Blue };

            for (int i = 0; i < codeNum; i++)
            {

                g.DrawString(retCode.Substring(i, 1), new Font("Arial", 25), Brushcolors[r.Next(0, Brushcolors.Length - 1)], i * 20, 0);

            }


            //for (int i = 0; i < 10; i++)
            //{

            //    Point[] arrary = { new Point(r.Next(0, 140), r.Next(0, 50)), new Point(r.Next(0, 140), r.Next(0, 50)), new Point(r.Next(0, 140), r.Next(0, 50)) };
            //    g.DrawPolygon(Pens.Gainsboro, arrary);

            //}


            //将图写到请求的地方
            MemoryStream ms = new MemoryStream();
            bit.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);

            context.Response.ContentType = "image/jpeg";

            context.Response.BinaryWrite(ms.ToArray());
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}