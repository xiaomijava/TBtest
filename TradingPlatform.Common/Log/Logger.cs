using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Common
{
    public class Logger
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void Debug(object message)
        {
            Logger.log.Debug(message);
        }

        public static void Debug(object message, Exception exception)
        {
            Logger.log.Debug(message, exception);
        }

        public static void DebugFormat(string format, object arg0)
        {
            Logger.log.DebugFormat(format, arg0);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            Logger.log.DebugFormat(format, args);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.log.DebugFormat(provider, format, args);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            Logger.log.DebugFormat(format, arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.log.DebugFormat(format, arg0, arg1, arg2);
        }

        public static void Error(object message)
        {
            Logger.log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            Logger.log.Error(message, exception);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            Logger.log.ErrorFormat(format, arg0);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            Logger.log.ErrorFormat(format, args);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.log.ErrorFormat(provider, format, args);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            Logger.log.ErrorFormat(format, arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public static void Fatal(object message)
        {
            Logger.log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            Logger.log.Fatal(message, exception);
        }

        public static void FatalFormat(string format, object arg0)
        {
            Logger.log.FatalFormat(format, arg0);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            Logger.log.FatalFormat(format, args);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.log.FatalFormat(provider, format, args);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            Logger.log.FatalFormat(format, arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.log.FatalFormat(format, arg0, arg1, arg2);
        }

        public static void Info(object message)
        {
            Logger.log.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
            Logger.log.Info(message, exception);
        }

        public static void InfoFormat(string format, object arg0)
        {
            Logger.log.InfoFormat(format, arg0);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            Logger.log.InfoFormat(format, args);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.log.InfoFormat(provider, format, args);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            Logger.log.InfoFormat(format, arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.log.InfoFormat(format, arg0, arg1, arg2);
        }

        public static void Warn(object message)
        {
            Logger.log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            Logger.log.Warn(message, exception);
        }

        public static void WarnFormat(string format, object arg0)
        {
            Logger.log.WarnFormat(format, arg0);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            Logger.log.WarnFormat(format, args);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Logger.log.WarnFormat(provider, format, args);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            Logger.log.WarnFormat(format, arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Logger.log.WarnFormat(format, arg0, arg1, arg2);
        }
    }
}