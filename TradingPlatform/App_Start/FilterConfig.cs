﻿using System.Web;
using System.Web.Mvc;

namespace TradingPlatform
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new GlobalHandleErrorAttribute());
        }
    }
}
