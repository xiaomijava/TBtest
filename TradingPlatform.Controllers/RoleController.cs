using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using TradingPlatform.IService;
using TradingPlatform.IService.Enum;
using TradingPlatform.Model;
using TradingPlatform.Model.Entities;
using TradingPlatform.Service;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    //[ControllerBase]
    public class RoleController : ControllerBase
    {
        RoleService roleServic = new RoleService();

        IUserService menuService = new UserService();

        /// <summary>
        /// 获取角色数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("Admin/GetByRoleList")]
        public JsonResult GetByRoleList(long id = 0)
        {
            if (id != 0)
            {
                ResponseList<long> respon = new ResponseList<long>();
                var date = menuService.Table.Where(t => t.IsDelete == false && t.Id == id).Select(t => t.RoleIds).FirstOrDefault().Split(',').ToList();
                respon.data = date.Select<string, long>(q => Convert.ToInt32(q)).ToList();
                respon.result = true;
                return Json(respon, JsonRequestBehavior.AllowGet);
            }
            else
            {
                ResponseList<Role> response = new ResponseList<Role>();
                response.data = roleServic.Table.Where(m => m.IsDelete == false).ToList();
                response.result = true;
                return Json(response, JsonRequestBehavior.AllowGet);
            }

        }

        //获取当前登录员工
        User user = System.Web.HttpContext.Current.Session["admin"] as User;



        /// <summary>
        /// 角色信息管理视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Role()
        {
            ViewBag.Title = "角色信息";
            return View();
        }

        /// <summary>
        /// 角色信息管理视图（Layui版）
        /// </summary>
        /// <returns></returns>
        public ActionResult LayuiRole()
        {
            return View();
        }

        /// <summary>
        /// 获取分页菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Role/GetRoleByPage")]
        public JsonResult GetRoleByPage(PageParam param, Role search)
        {
            int total = 0;
            ResponseList<Role> response = new ResponseList<Role>();
            Expression<Func<Role, bool>> where = c => true;
            where = where.And(m => m.IsDelete == false);
            if (!string.IsNullOrWhiteSpace(search.Name))
            {
                where = where.And(m => m.Name.Contains(search.Name));
            }
            if (!string.IsNullOrWhiteSpace(search.CreateTime.ToString()))
            {
                where = where.And(m => m.CreateTime >= search.CreateTime);
            }
            if (!string.IsNullOrWhiteSpace(search.UpdateTime.ToString()))
            {
                where = where.And(m => m.CreateTime <= search.UpdateTime);
            }
            List<Role> menus = new List<Role>();
            menus = roleServic.LoadPageItems<DateTime?>(param.pageSize, param.pageIndex, out total, where, c => c.CreateTime, param.IsAsc).ToList();
            response.result = true;
            response.data = menus;
            return Json(new { responseData = response, total = total }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 添加角色信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/Role/addRole")]
        public JsonResult addRole(Role model)
        {
            Response<Role> response = new Response<Role>();


            if (model == null)
            {
                response.result = false;
                response.message = "参数不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }


            if (roleServic.Insert(model) > 0)
            {
                response.result = true;
                response.message = "添加成功!";
            }
            else
            {
                response.result = false;
                response.message = "添加失败!";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }



        ///// <summary>
        ///// 修改用户信息
        ///// </summary>
        ///// <returns></returns>
        [HttpPost]
        [Route("/Role/updateRole")]
        public JsonResult updateRole(Role model, string MenuIDs = null)
        {
            Response<Role> response = new Response<Role>();

            if (model == null)
            {
                response.message = "参数不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            Role role = roleServic.GetById(model.Id);
            if (MenuIDs != null)
            {
                role.MenuIDs = MenuIDs;
            }
            else
            {
                role.Name = model.Name;
                role.Level = model.Level;
                role.Desc = model.Desc;
            }

            if (roleServic.Update(role) > 0)
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

        ///// <summary>
        ///// 删除用户信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[System.Web.Http.Route("/Admin/deleteAdmin")]
        //public JsonResult deleteAdmin(List<Admin> model)
        //{
        //    Response<Admin> response = new Response<Admin>();
        //    if (model == null)
        //    {
        //        response.result = false;
        //        response.message = "参数不能为空!";
        //        return Json(response, JsonRequestBehavior.AllowGet);
        //    }

        //    if (_menuService.Delete(model, IService.Enum.EnumDeleteType.Logically) > 0)
        //    {
        //        response.result = true;
        //        response.message = "删除成功!";
        //    }
        //    else
        //    {
        //        response.result = false;
        //        response.message = "删除失败!";
        //    }
        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}
    }
}
