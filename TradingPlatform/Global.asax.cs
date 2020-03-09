using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;
using TradingPlatform.Common;

namespace TradingPlatform
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            log4net.Config.XmlConfigurator.Configure(new System.IO.FileInfo(Server.MapPath("~/log4net.config")));
            Logger.Info("Server Started 20161031...");
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            // 在新会话启动时运行的代码。
            // 该方法被调用，表示当前有一个新的会话产生了。
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            // 在新请求启动时允许的代码。
            // 每一次请求都会触发该方法。
            // 取得当前请求url：HttpContext.Current.Request.Url
           
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            // 在应用程序启动时运行的代码。
            // 程序第一次获得请求时，该方法被执行。
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码
            // 获取异常信息并处理：HttpContext.Current.Server.GetLastError();
        }

        protected void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为InProc 时，才会引发 Session_End 事件。
            // 如果会话模式设置为 StateServer  或 SQLServer，则不会引发该事件。
        }

        protected void Application_End(object sender, EventArgs e)
        {
            //  在应用程序关闭时运行的代码
            //Session["admin"] = null;
            //Session.Remove("admin");
        }


    }
}
