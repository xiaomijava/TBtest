using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TradingPlatform.Common;
using TradingPlatform.IService;
using TradingPlatform.Model;
using TradingPlatform.Service;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    public class MemberNewsController : ControllerOldMemberBase
    {
        INewsService _menuService = new NewsService();

        static int Newclass;
        /// <summary>
        /// 公告页面
        /// </summary>
        /// <returns></returns>
        public ActionResult NewsIndex(int id)
        {
            Newclass = id;
            if (id == 1)
            {
                ViewBag.Title = "公司新闻";
            }
            else
            {
                ViewBag.Title = "系统公告";
            }
            return View();
        }


        /// <summary>
        /// 获取分页菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MemberNews/GetNewseByPage")]
        public JsonResult GetNewseByPage(PageParam param)
        {
            int total = 0;
            ResponseList<News> response = new ResponseList<News>();
            Expression<Func<News, bool>> where = c => true;
            where = where.And(m => m.IsDelete == false);
            where = where.And(m => m.New_Class == Newclass);
            where = where.And(m => m.IsEnable == true);
            List<News> menus = new List<News>();
            menus = _menuService.LoadPageItems<DateTime?>(param.pageSize, param.pageIndex, out total, where, c => c.CreateTime, param.IsAsc).ToList();
            response.result = true;
            response.data = menus;
            return Json(new { responseData = response, total = total }, JsonRequestBehavior.AllowGet);
        }
    }
}
