using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TradingPlatform.Common.IP
{
    public class IPTools
    {
        public static string GetIP(HttpRequestBase Request) {
            string ip;
            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] == null)
            {
                ip = Request.ServerVariables["REMOTE_HOST"].ToString();
            }
            else
            {
                ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            return ip;
        }
        public static string GetIPAddress(string cIp)
        {
            string ip = cIp.Split(new char[] { ',' })[0];
            //ip对应城市
            string ipAddress = "未知";
            IPScanner objScan = new IPScanner();
            objScan.IP = ip;
            ipAddress = objScan.IPLocation();
            return ipAddress;
        }
    }
}
