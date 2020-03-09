using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using TradingPlatform.IService.Enum;
using TradingPlatform.Model;
using TradingPlatform.Model.Entities;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    //[ControllerBase]
    public class NewsController : ControllerBase
    {
        NewsService _menuService = new NewsService();

        /// <summary>
        /// 新闻管理视图
        /// </summary>
        /// <returns></returns>
        public ActionResult News()
        {
            ViewBag.Title = "新闻列表";
            return View();
        }


        /// <summary>
        /// 添加新闻信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [System.Web.Http.Route("/News/addNews")]
        public JsonResult addNews(News model)
        {
            Response<News> response = new Response<News>();
            if (model == null)
            {
                response.message = "参数不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (_menuService.Insert(model) > 0)
            {
                response.result = true;
                response.message = "添加成功!";
            }
            else
            {
                response.message = "添加失败!";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 修改新闻信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [System.Web.Http.Route("/News/updateNews")]
        public JsonResult updateNews(News model)
        {
            Response<News> response = new Response<News>();
            if (model == null)
            {
     
                response.message = "参数不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            News ne = _menuService.GetById(model.Id);
            ne.New_Title = model.New_Title;

            ne.New_Content = model.New_Content;

            //ne.IsTop = model.IsTop;

            ne.IsEnable = model.IsEnable;

            if (_menuService.Update(ne) > 0)
            {
                response.result = true;
                response.message = "修改成功!";
            }
            else
            {
                response.message = "修改失败!";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 删除新闻信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult deleteNews(List<News> model)
        {
            Response<News> response = new Response<News>();
            if (model == null)
            {
                response.message = "参数不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            if (_menuService.Delete(model, IService.Enum.EnumDeleteType.Logically) > 0)
            {
                response.result = true;
                response.message = "删除成功!";
            }
            else
            {
                response.message = "删除失败!";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取分页菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("News/GetNewsByPage")]
        public JsonResult GetNewsByPage(PageParam param, News search)
        {
            int total = 0;
            ResponseList<News> response = new ResponseList<News>();
            Expression<Func<News, bool>> where = c => true;
            where = where.And(m => m.IsDelete == false);
            if (!string.IsNullOrWhiteSpace(search.New_Title))
            {
                where = where.And(m => m.New_Title.Contains(search.New_Title));
            }
            if (!string.IsNullOrWhiteSpace(search.CreateTime.ToString()))
            {
                where = where.And(m => m.CreateTime >= search.CreateTime);
            }
            if (!string.IsNullOrWhiteSpace(search.UpdateTime.ToString()))
            {
                where = where.And(m => m.CreateTime <= search.UpdateTime);
            }

            List<News> menus = new List<News>();
            menus = _menuService.LoadPageItems<DateTime?>(param.pageSize, param.pageIndex, out total, where, c => c.CreateTime, param.IsAsc).ToList();
            response.result = true;
            response.data = menus;
            return Json(new { responseData = response, total = total }, JsonRequestBehavior.AllowGet);
        }
    }
}
