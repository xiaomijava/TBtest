using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TradingPlatform
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            //忽略路由
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.Add("chrome", new Elevent());

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Login", id = UrlParameter.Optional }
                ,namespaces: new string[] { "TradingPlatform.Controllers" }
            );
        }
    }

    /// <summary>
    /// 过滤，程序启动时执行
    /// </summary>
    public class Elevent : RouteBase
    {
        public override RouteData GetRouteData(HttpContextBase httpContext)
        {
            if (httpContext.Request.UserAgent.Contains("Chrome"))
            {
                return null;
            }
            else
            {
                RouteData routeData = new RouteData(this, new MvcRouteHandler());
                routeData.Values.Add("controller", "Home");
                routeData.Values.Add("action", "Index2");
                return routeData;
            }
        }

        public override VirtualPathData GetVirtualPath(RequestContext requestContext, RouteValueDictionary values)
        {
            return null;
        }
    }
}
