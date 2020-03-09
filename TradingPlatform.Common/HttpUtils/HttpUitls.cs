using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace TradingPlatform.Common
{
    public class HttpUitls
    {
        /// <summary>
        /// 异步Get请求
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        public static string Get(string Url)
        {
            //System.GC.Collect();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Proxy = null;
            request.KeepAlive = false;
            request.Method = "GET";
            request.ContentType = "application/json; charset=UTF-8";
            request.AutomaticDecompression = DecompressionMethods.GZip;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream myResponseStream = response.GetResponseStream();
            StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
            string retString = myStreamReader.ReadToEnd();

            myStreamReader.Close();
            myResponseStream.Close();

            if (response != null)
            {
                response.Close();
            }
            if (request != null)
            {
                request.Abort();
            }

            return retString;
        }
        /// <summary>
        /// 异步Post请求
        /// </summary>
        /// <param name="requestUrl">请求地址</param>
        /// <param name="data">参数</param>
        /// <returns></returns>
        public static string HttpWebPost(string requestUrl, Dictionary<string, string> data)
        {
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(requestUrl);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";

            StringBuilder builder = new StringBuilder();
            int i = 0;
            foreach (var item in data)
            {
                if (i > 0)
                    builder.Append("&");
                builder.AppendFormat("{0}={1}", item.Key, item.Value);
                i++;
            }
            byte[] bt = Encoding.UTF8.GetBytes(builder.ToString());
            req.ContentLength = bt.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bt, 0, bt.Length);
                reqStream.Close();
            }

            string responseData;
            using (HttpWebResponse response = (HttpWebResponse)req.GetResponse())
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    responseData = reader.ReadToEnd().ToString();
                }
            }

            return responseData;
        }
    }
}