
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TradingPlatform.Common;
using TradingPlatform.IService;
using TradingPlatform.Model;
using TradingPlatform.Model.Entities;
using TradingPlatform.Service;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    //[ControllerBase]
    public class UserController : ControllerBase
    {
        IUserService _menuService = new UserService();
        User user = System.Web.HttpContext.Current.Session["admin"] as User;
        IFinanceService financeService = new FinanceService();
        /// <summary>
        /// 会员信息视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin()
        {
            ViewBag.Title = "用户列表";
            return View();
        }

        /// <summary>
        /// 主页
        /// </summary>
        public ActionResult Homeindex()
        {
            return View();
        }

        /// <summary>
        /// 首页视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Onepage()
        {
            User users = _menuService.GetById(user.Id);
            //获取投资金额
            ViewBag.OneMoney = users.OneMoney == null ? 0 : users.OneMoney;

            //总收入
            var sumcount = financeService.Table.Where(t => t.User_Number == users.User_Number && t.IsDelete == false).Sum(t => t.Amount);

            if (sumcount == null)
            {
                sumcount = 0;
            }
            ViewBag.Sum = sumcount;

            //账户余额
            ViewBag.total = Convert.ToInt32(ViewBag.OneMoney) + sumcount;
            ViewBag.usernumber = users.User_Number;
            ViewBag.username = users.User_Name;
            ViewBag.time = users.UpdateTime==null?null: users.UpdateTime.Value.ToString("yyyy-MM-dd");
            return View();
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <returns></returns>
        public JsonResult updateUserUrl()
        {
            Response<User> response = new Response<User>();

            if (Request.Files.Count > 0)//验证是否包含文件
            {
                HttpPostedFileBase pic_upload = Request.Files[0];
                //取得文件的扩展名,并转换成小写
                string fileExtension = Path.GetExtension(pic_upload.FileName).ToLower();
                //对上传文件的大小进行检测，限定文件最大不超过8M
                if (pic_upload.ContentLength < 8192000)
                {
                    string filepath = "/static/urlimages/" + DateTime.Now.ToString("yyyy-MM-dd") + "/";

                    if (Directory.Exists(Server.MapPath(filepath)) == false)//如果不存在就创建file文件夹
                    {
                        Directory.CreateDirectory(Server.MapPath(filepath));
                    }
                    string mappath =filepath + pic_upload.FileName;//图片路径

                    pic_upload.SaveAs(Server.MapPath(mappath));//保存图片

                    User user = _menuService.GetById(((User)Session["admin"]).Id);
                    user.User_Url = mappath;

                    if (_menuService.Update(user) > 0)
                    {
                        response.result = true;
                        response.message = "图片上传成功";

                        System.Web.HttpContext.Current.Session["admin"] = user;
                    }
                    else
                    {
                        response.message = "图片上传失败";
                    }

                }
                else
                {
                    response.message = "文件大小超出8M！请重新选择！";
                }
            }
            else
            {
                response.message = "请选择要上传的图片！";
            }
          
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 用户详情
        /// </summary>
        /// <returns></returns>
        public ActionResult UserIndex()
        {
            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult UserPass()
        {
            return View();
        }

        /// <summary>
        /// 添加会员信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [System.Web.Http.Route("/User/addUser")]
        public JsonResult addUser(User model)
        {
            Response<User> response = new Response<User>();
            if (model == null)
            {
                response.message = "参数不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrWhiteSpace(model.RoleIds))
            {
                response.message = "请至少绑定一个角色!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            //加密
            model.U_1st_Password = MD5Utils.MD5Encrypt32(model.U_1st_Password);
            model.U_2st_Password = MD5Utils.MD5Encrypt32(model.U_2st_Password);
            model.U_3st_Password = MD5Utils.MD5Encrypt32(model.U_3st_Password);

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
        /// 修改会员信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [System.Web.Http.Route("/User/updateUser")]
        public JsonResult updateUser(User model)
        {
            Response<User> response = new Response<User>();
            if (model == null)
            {
                response.message = "参数不能为空!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            if (string.IsNullOrWhiteSpace(model.RoleIds))
            {
                response.message = "请至少绑定一个角色!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            User user = _menuService.GetById(model.Id);

            if (model.U_Passport == "log")
            {
                if (MD5Utils.MD5Encrypt32(model.U_1st_Password) == user.U_1st_Password)
                { user.U_1st_Password = MD5Utils.MD5Encrypt32(model.U_2st_Password); }
                else
                {
                    response.message = "当前密码不正确!";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                user.User_Name = model.User_Name;
                user.User_Sex = model.User_Sex;
                user.User_QQ = model.User_QQ;
                user.User_NickName = model.User_NickName;
                user.User_Email = model.User_Email;
                user.Bindphone = model.Bindphone;
                user.RoleIds = model.RoleIds;
            }
            if (_menuService.Update(user) > 0)
            {
                response.result = true;
                response.message = "修改成功!";
                System.Web.HttpContext.Current.Session["admin"] = user;
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
        [System.Web.Http.Route("/User/deleteUser")]
        public JsonResult deleteUser(List<User> model)
        {
            Response<User> response = new Response<User>();
            if (model == null)
            {
                response.result = false;
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
                response.result = false;
                response.message = "删除失败!";
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("User/GetUserByCount")]
        public JsonResult GetUserByCount()
        {

            ResponseList<User> response = new ResponseList<User>();
            response.test = _menuService.GetById(((User)Session["admin"]).Id);
            return Json(new { responseData = response }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取分页菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("User/GetUserByPage")]
        public JsonResult GetUserByPage(PageParam param, User search)
        {
            int total = 0;
            ResponseList<User> response = new ResponseList<User>();
            Expression<Func<User, bool>> where = c => true;
            where = where.And(m => m.IsDelete == false);
            if (!string.IsNullOrWhiteSpace(search.User_Name))
            {
                where = where.And(m => m.User_Name.Contains(search.User_Name));
            }
            if (!string.IsNullOrWhiteSpace(search.User_Number))
            {
                where = where.And(m => m.User_Number.Contains(search.User_Number));
            }
            if (!string.IsNullOrWhiteSpace(search.User_NickName))
            {
                where = where.And(m => m.User_NickName.Contains(search.User_NickName));
            }
            if (!string.IsNullOrWhiteSpace(search.User_QQ))
            {
                where = where.And(m => m.User_Name.Contains(search.User_QQ));
            }
            if (!string.IsNullOrWhiteSpace(search.CreateTime.ToString()))
            {
                where = where.And(m => m.CreateTime >= search.CreateTime);
            }
            if (!string.IsNullOrWhiteSpace(search.UpdateTime.ToString()))
            {
                where = where.And(m => m.UpdateTime <= search.UpdateTime);
            }

            List<User> menus = new List<User>();
            menus = _menuService.LoadPageItems<DateTime?>(param.pageSize, param.pageIndex, out total, where, c => c.CreateTime, param.IsAsc).ToList();
            response.result = true;
            response.data = menus;
            return Json(new { responseData = response, total = total }, JsonRequestBehavior.AllowGet);
        }
    }
}

