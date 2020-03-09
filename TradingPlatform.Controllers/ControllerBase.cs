using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TradingPlatform.Model;
using TradingPlatform.Model.Entities;

namespace TradingPlatform.Controllers
{
    //管理员base控制器
    public class ControllerBase : Controller
    {
        public static User adminInfo = null;
        //public override void OnAuthorization(AuthorizationContext filterContext)
        //{
        //    if (HttpContext.Current.Session["admin"] == null)
        //    {
        //        filterContext.HttpContext.Response.Redirect("/Longin.html");
        //    }
        //    else {
        //       adminInfo = System.Web.HttpContext.Current.Session["list"] as Admin;
        //    }
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

            adminInfo = Session["admin"] as User;
            if (adminInfo == null)
            {
                //重定向至登录页面
                //filterContext.HttpContext.Response.Redirect("/Longin.html");
                //filterContext.Result = RedirectToAction("Index", "Login", new { url = Request.RawUrl });
                //filterContext.Result = Redirect("/Login/Login");

                //filterContext.HttpContext.Response.Write("<script>top.location.href='/Login/Login'</script>");

                ContentResult Content = new ContentResult();
                Content.Content = string.Format("<script type='text/javascript'>top.location.href='{0}';</script>", "/Login/Login");
                filterContext.Result = Content;

                return;
            }

        }
    }
}
