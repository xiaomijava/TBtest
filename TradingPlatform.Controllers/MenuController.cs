using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TradingPlatform.IService.Enum;
using TradingPlatform.Model;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    //[ControllerBase]
    public class MenuController : ControllerBase
    {
        MenuService _menuService = new MenuService();
        public ActionResult Menu()
        {
            ViewBag.Title= "菜单列表";
            return View();
        }

        /// <summary>
        /// 获取当前管理员权限下的菜单
        /// </summary>
        /// <returns></returns>
        public JsonResult GetAdminMenus() {

            Response<object> response = new Response<object>();

            var menus = _menuService.GetMenueList(adminInfo.Id);
            response.result = true;
            response.data = menus;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取一级菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Menu/getAllMenus")]
        public JsonResult GetAllMenus() {
            ResponseList<Menu> response = new ResponseList<Menu>();
            response.data = _menuService.Table.Where(m=>m.IsDelete==false && m.Parent_ID==0).ToList();
            response.result = true;
            return Json(response,JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取导航页菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Menu/getMenus")]
        public JsonResult GetMenus()
        {
            ResponseList<ResponseMenus> response = new ResponseList<ResponseMenus>();
            List<ResponseMenus> responseMenus = new List<ResponseMenus>();
            List<Menu> menus = new List<Menu>();

            menus = _menuService.Table.OrderBy(m => m.CreateTime).ToList();
            foreach (var menu in menus.Where(m => m.Parent_ID == 0 && m.IsDelete==false))
            {
                ResponseMenus responseMenu = new ResponseMenus();
                responseMenu.menu = menu;

                responseMenu.sonmenus = menus.Where(m => m.Parent_ID == menu.Id && m.IsDelete == false).OrderBy(n => n.CreateTime).ToList();
                if (responseMenu.sonmenus.Count() > 0)
                {
                    responseMenu.key = true;
                }
                else
                {
                    responseMenu.key = false;
                }
                responseMenus.Add(responseMenu);
            }
            response.result = true;
            response.data = responseMenus;
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 获取分页菜单
        /// </summary>
        /// <returns></returns>
        public JsonResult GetMenusByPage(PageParam param, string searchName)
        {
            int total = 0;
            Response<dynamic> response = new Response<dynamic>();
            var data = (from a in _menuService.TableNoTracking.Where(o => o.IsDelete == false)
                        select new {
                            Id=a.Id,
                            Menu_Name = a.Menu_Name,
                            Parent_ID = a.Parent_ID,
                            Parent_Menu = a.Parent_ID==null ? "": _menuService.TableNoTracking.FirstOrDefault(o=>o.Id==a.Parent_ID).Menu_Name,
                            Menu_Path = a.Menu_Path,
                            ZIndex = a.ZIndex,
                            Menu_Icon =a.Menu_Icon,
                            CreateTime = a.CreateTime
                        });

            if (!string.IsNullOrWhiteSpace(searchName))
            {
                data = data.Where(o => o.Menu_Name == searchName);
            }

            total = data.Count();
            //menus = _menuService.Table.OrderBy(m => m.CreateTime).ToList();
            
            response.result = true;
            response.data = data.OrderByDescending(o=>o.CreateTime).Skip(param.pageSize*(param.pageIndex-1)).Take(param.pageSize);
            return Json(new { responseData=response,total=total }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Menu/addMenu")]
        public JsonResult AddMenu(Menu model)
        {
            Response<Menu> response = new Response<Menu>();
            if (model == null)
            {
                response.result = false;
                response.message = "参数不能为空!";
                return Json(response,JsonRequestBehavior.AllowGet);
            }
            if (_menuService.Insert(model) > 0)
            {
                response.result = true;
                response.message = "添加成功!";
            }
            else {
                response.result = false;
                response.message = "添加失败!";
            }
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Menu/GetByMenuList")]
        public JsonResult GetByMenuList()
        {
            ResponseList<Menu> response = new ResponseList<Menu>();
            response.result = true;
            response.data = _menuService.Table.Where(t => t.IsDelete == false).ToList();
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        
        /// <summary>
        /// 修改菜单
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("Menu/UpdateMenu")]
        public JsonResult UpdateMenu(Menu model)
        {
            Response<Menu> response = new Response<Menu>();
            if (model == null)
            {
                response.result = false;
                response.message = "参数不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (model.Id == 0)
            {
                response.result = false;
                response.message = "ID不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            var menu = _menuService.GetById(model.Id);
            if (menu == null)
            {
                response.result = false;
                response.message = "无法获取菜单信息!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            menu.Menu_Name = model.Menu_Name;
            menu.Menu_Path = model.Menu_Path;
            menu.Menu_Icon = model.Menu_Icon;
            menu.Parent_ID = model.Parent_ID;
            menu.ZIndex = model.ZIndex;
            if (_menuService.Update(menu) > 0)
            {
                response.result = true;
                response.message = "修改成功!";
            }
            else
            {
                response.result = false;
                response.message = "修改失败!";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("Menu/deleteMenu")]
        public JsonResult DeleteMenu(string ids) {
            Response<Menu> response = new Response<Menu>();
            if (string.IsNullOrWhiteSpace(ids))
            {
                response.result = false;
                response.message = "参数不能为空!";
                return Json(response,JsonRequestBehavior.AllowGet);
            }
            string[] strArray = ids.Split(',').ToArray();
            List<Menu> menus = _menuService.Table.Where(m => strArray.Contains(m.Id.ToString())).ToList();
            if (menus == null || menus.Count() == 0)
            {
                response.result = false;
                response.message = "获取不到实体!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (_menuService.Delete(menus, EnumDeleteType.Logically) > 0)
            {
                response.result = true;
                response.message = "删除成功!";
            }
            else
            {
                response.result = false;
                response.message = "删除失败!";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
