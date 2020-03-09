using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using TradingPlatform.Common;

namespace System.Web
{
    /// <summary>
    /// Controller Error Filter
    /// </summary>
    public class GlobalHandleErrorAttribute : System.Web.Mvc.HandleErrorAttribute
    {
        public override void OnException(System.Web.Mvc.ExceptionContext filterContext)
        {
            //If the exeption is already handled we do nothing
            filterContext.Result = new JsonResult()
            {
                Data = new { status = 500, title = "提示", message = filterContext.Exception.Message, data = new List<Object>(), total = 0 },
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
            Logger.Error("Controller Error:" + filterContext.Exception.Message, filterContext.Exception);
            filterContext.ExceptionHandled = true;
        }
    }
}