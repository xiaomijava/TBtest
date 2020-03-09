using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using TradingPlatform.Common;

namespace TradingPlatform.Web
{
    /// <summary>
    /// Web Api Filter
    /// </summary>
    public class GlobalExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotImplemented);
            }

            Logger.Error("WebApi Error:" + context.Exception.Message, context.Exception);
        }
    }
}