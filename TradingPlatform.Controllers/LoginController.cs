
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TradingPlatform.Common;
using TradingPlatform.Common.IP;
using TradingPlatform.Common.Message;
using TradingPlatform.IService;
using TradingPlatform.Model;
using TradingPlatform.Model.Entities;
using TradingPlatform.Service;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    public class LoginController : Controller
    {
        IUserService _menuService = new UserService();

        /// <summary>
        /// 登录页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        public string GetConnLongin(string name, string pwd, string yzm)
        {
            //获取session对象验证码
            string y = (string)System.Web.HttpContext.Current.Session["retCode"];
            //定义变量
            string str = null;
            //判断
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
                str = "账号或密码不能为空！";
            else
            {
                if (!string.Equals(y, yzm))
                    str = "验证码输入错误！";
                else
                {
                    //md5加密
                    pwd = MD5Utils.MD5Encrypt32(pwd);
                    //调用方法返回对象
                    User admin = _menuService.Table.FirstOrDefault(t => t.User_Number == name && t.U_1st_Password == pwd && t.IsDelete == false);
                    //判断对象是否为空
                    if (admin == null)
                        str = "账号或密码错误！";
                    else
                    {
                        //把对象存入session中
                        Session["admin"] = admin;
                        Session["SourceName"] = "管理员：" + admin.User_Number;
                        str = "ok";

                        //打印日志
                        var logs = $"账号为：{name} 会员在{DateTime.Now.ToString()}时登入成功！";
                        logs.Writelog();
                    }
                }
            }
            return str;
        }
    }

}

