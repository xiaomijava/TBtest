using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace TradingPlatform.Model
{
   public static class OutPutLog
    {
        #region 系统自带的日志接口
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Info(this object message)
        {
            log.Info(message);
        }
        #endregion
        public static void Writelog(this string msg)
        {
            StreamWriter stream;
            //写入日志内容
            string path = AppDomain.CurrentDomain.BaseDirectory + "logFile\\";
            //检查上传的物理路径是否存在，不存在则创建
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string logFileName = (DateTime.Now.Year).ToString() + '-'
              + (DateTime.Now.Month).ToString() + '-' + (DateTime.Now.Day).ToString() + "_Log.txt";
            stream = new StreamWriter(path + $"\\{logFileName}", true, Encoding.Default);
            stream.Write("\n"+msg+ "\r\n");
            stream.Flush();
            stream.Close();
        }
    }
}
