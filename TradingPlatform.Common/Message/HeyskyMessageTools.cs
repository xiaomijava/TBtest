using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Common.Message
{
    public class HeyskyMessageTools
    {
        private static string heyskyURL = ConfigurationManager.AppSettings["heyskyURL"]; //接口地址
        private static string  heyskyAccount = ConfigurationManager.AppSettings["heyskyAccount"];//账号
        private static string heyskyPassword = ConfigurationManager.AppSettings["heyskyPassword"];//密码

        /// <summary>
        /// 发送匹配成功提示短信
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="userNumber">会员账号</param>
        /// <param name="orderNumber">订单号</param>
        public static void SendMassege(string phoneNumber,string userNumber, string orderNumber,string biaoshi=null) {
            var account = heyskyAccount;
            var password = heyskyPassword;
            var PostUrl = heyskyURL;
            phoneNumber = "86" + phoneNumber;
            var Template = "尊贵的会员：[num],您的订单[order]于[time]已成功匹配。请知悉,尽快完成交易!";
            var result = Template.Replace("[num]", userNumber).Replace("[order]", orderNumber).Replace("[time]", DateTime.Now.ToString());

            //if (result != null)
            //{
            //    result = "尊贵的会员："+ userNumber + ",您的验证码为:"+ orderNumber + ",请尽快验证!";
            //}
            //var result ="";


            #region
            //todo
            #endregion
            string format1 = "cpid={0}&cppwd={1}&da={2}&sm={3}";
            var a = string.Format(format1, new object[]
                       {
                        account,
                        password,
                        phoneNumber,
                        result
             });
            var url = PostUrl + "&" + a;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Method = "Get";
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";

                HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                Stream myResponseStream = httpWebResponse.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.UTF8);
                string retString = myStreamReader.ReadToEnd();
                if (httpWebResponse.StatusCode == HttpStatusCode.OK)
                {
                    Logger.Info("发送成功：" + result + "返回信息：" + retString);
                }
                else
                {
                    Logger.Error("发送失败：" + result + "返回信息：" + retString);
                }
            }
            catch (Exception ex)
            {
                Logger.Error("请求短信接口发生错误，会员账户:" + userNumber + "  ，错误信息" + ex.Message);
            }
        }
    }
}
