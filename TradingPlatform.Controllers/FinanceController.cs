
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using TradingPlatform.Common;
using TradingPlatform.IService;
using TradingPlatform.Model;
using TradingPlatform.Model.Entities;
using TradingPlatform.Service;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    public class My
    {
        public long id { get; set; }

        public int time { get; set; }
        public double rate { get; set; }

        public string pass { get; set; }

        public decimal money { get; set; }
    }


    public class FinanceController : ControllerBase
    {
        IFinanceService financeService = new FinanceService();
        IUserService userService = new UserService();
        User user = System.Web.HttpContext.Current.Session["admin"] as User;
        static Thread thread = null;
        /// <summary>
        /// 流水视图
        /// </summary>
        /// <returns></returns>
        public ActionResult FinanceIndex()
        {
            var sum = financeService.Table.Where(t => t.User_Number == user.User_Number && t.IsDelete == false).Sum(t => t.Amount);

            ViewBag.sum = sum;
            return View();
        }
        /// <summary>
        /// 投资金额视图
        /// </summary>
        /// <returns></returns>
        public ActionResult MoneyIndex()
        {
            return View();
        }

        /// <summary>
        /// 计算利息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public JsonResult Calculation(My my)
        {
            Response<User> response = new Response<User>();

            //把本金存入
            User user = userService.GetById(((User)Session["admin"]).Id);
            if (User == null)
            {
                response.message = "没有获取到用户信息!";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                if (user.U_3st_Password != MD5Utils.MD5Encrypt32(my.pass))
                {
                    response.message = "三级密码不正确!";
                    return Json(response, JsonRequestBehavior.AllowGet);
                }
                user.Money = my.money;
                user.OneMoney = my.money;
            }

            my.id = user.Id;

            thread = new Thread(new ParameterizedThreadStart(Test));
            thread.Start(my);

            //Task.Run(()=>Test(my));

            response.result = true;
            response.message = "投资成功!";
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public string FlagTest()
        {
            string falg = "no";

            try
            {
                if (thread.ThreadState == ThreadState.Aborted)
                {
                    falg = "ok";
                }
                else
                {
                    thread.Abort();
                }
            }
            catch (Exception)
            {

                falg = "log";
            }


            return falg;
        }

        public void Test(object obj)
        {
            My my = (My)obj;

            while (true)
            {
                Thread.Sleep(1000 * my.time);
                //把本金存入
                User user = userService.GetById(my.id);
                //计算利息
                Finance finance = new Finance();
                finance.User_Number = user.User_Number;
                finance.TypeID = 1;
                finance.Principal = user.Money.Value;
                finance.Amount = Convert.ToDecimal(user.Money * Convert.ToDecimal(my.rate * 0.00001));
                user.Money += finance.Amount;
                using (TransactionScope trans = new TransactionScope())//使用事务
                {
                    try
                    {
                        userService.Update(user);
                        financeService.Insert(finance);
                        trans.Complete();
                        trans.Dispose();
                    }
                    catch (Exception ex)
                    {
                        return;
                    }

                }
            }
        }

        /// <summary>
        /// 获取分页菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("Finance/GetUserByPage")]
        public JsonResult GetUserByPage(PageParam param, Finance search)
        {
            int total = 0;
            ResponseList<Finance> response = new ResponseList<Finance>();
            Expression<Func<Finance, bool>> where = c => true;
            where = where.And(m => m.IsDelete == false);

            where = where.And(m => m.User_Number == user.User_Number);

            if (!string.IsNullOrWhiteSpace(search.User_Number))
            {
                where = where.And(m => m.User_Number == search.User_Number);
            }
            if (!string.IsNullOrWhiteSpace(search.CreateTime.ToString()))
            {
                where = where.And(m => m.CreateTime >= search.CreateTime);
            }
            if (!string.IsNullOrWhiteSpace(search.UpdateTime.ToString()))
            {
                where = where.And(m => m.UpdateTime <= search.UpdateTime);
            }

            List<Finance> menus = new List<Finance>();
            menus = financeService.LoadPageItems<DateTime?>(param.pageSize, param.pageIndex, out total, where, c => c.CreateTime, param.IsAsc).ToList();
            response.result = true;
            response.data = menus;
            return Json(new { responseData = response, total = total }, JsonRequestBehavior.AllowGet);
        }



        /// <summary>
        /// 导出数据
        /// </summary>
        /// <returns></returns>
        [Route("Finance/dataToExcel")]
        public FileStreamResult DataToExcel(string User_Number, string CreateTime, string UpdateTime)
        {

            Expression<Func<Finance, bool>> where = c => true;
            where = where.And(m => m.IsDelete == false);

            if (!string.IsNullOrWhiteSpace(User_Number))
            {
                where = where.And(m => m.User_Number == User_Number);
            }
            if (CreateTime != "NaN")
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                DateTime starDtae = startTime.AddMilliseconds(Convert.ToInt64(CreateTime));
                starDtae = starDtae.AddDays(-1);
                where = where.And(o => o.CreateTime > starDtae);
            }
            if (UpdateTime != "NaN")
            {
                DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
                DateTime endDtae = startTime.AddMilliseconds(Convert.ToInt64(UpdateTime));
                endDtae = endDtae.AddDays(1);
                where = where.And(o => o.CreateTime < endDtae);
            }

            var FinanceList = financeService.Table.Where(where).ToList();

            //创建Exce1工作簿
            HSSFWorkbook exce1Book = new HSSFWorkbook();
            //创建工作表并命名CreateSheet: 命名的方法
            ISheet sheet1 = exce1Book.CreateSheet("流水信息");
            //创建表头行CreateRow(0): 获取工作表的第-行.
            IRow rewl = sheet1.CreateRow(0);
            ICell Icell = rewl.CreateCell(0);
            Icell.SetCellValue("利滚利流水信息表");

            ICellStyle style = exce1Book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            style.Alignment = HorizontalAlignment.Center;
            //新建一个字体样式对象
            IFont font = exce1Book.CreateFont();
            font.FontName = "宋体";
            font.FontHeightInPoints = 18;
            //设置字体加粗样式
            font.Boldweight = (short)FontBoldWeight.Bold;
            //使用SetFont方法将字体样式添加到单元格样式中 
            style.SetFont(font);
            //将新的样式赋给单元格
            Icell.CellStyle = style;
            //合并单元格
            sheet1.AddMergedRegion(new CellRangeAddress(0, 0, 0, 4));


            string[] headName = { "ID", "会员编号", "本金", "利息", "生成效益时间" };
            #region   表头

            //表头样式
            #region
            ICellStyle Istyle2 = exce1Book.CreateCellStyle();
            //设置边框
            Istyle2.BorderTop = BorderStyle.Thin;
            Istyle2.BorderBottom = BorderStyle.Thin;
            Istyle2.BorderLeft = BorderStyle.Thin;
            Istyle2.BorderRight = BorderStyle.Thin;
            //设置单元格的样式：水平对齐居中
            Istyle2.Alignment = HorizontalAlignment.Center;
            //新建一个字体样式对象
            IFont Ifont2 = exce1Book.CreateFont();
            Ifont2.FontName = "宋体";
            Ifont2.FontHeightInPoints = 11;
            //设置字体加粗样式
            Ifont2.Boldweight = (short)FontBoldWeight.Bold;
            //使用SetFont方法将字体样式添加到单元格样式中 
            Istyle2.SetFont(Ifont2);
            #endregion

            //创建表头行CreateRow(1): 获取工作表的第2行.
            IRow rew2 = sheet1.CreateRow(1);
            for (int j = 0; j < 5; j++)
            {
                ICell Icell2 = rew2.CreateCell(j);
                //将新的样式赋给单元格
                Icell2.CellStyle = Istyle2;
                Icell2.SetCellValue(headName[j]);
            }
            #endregion

            ICellStyle Istyle3 = exce1Book.CreateCellStyle();
            //设置单元格的样式：水平对齐居中
            Istyle3.Alignment = HorizontalAlignment.Center;
            //为Exce1表格添加数据
            for (int i = 0; i < FinanceList.Count; i++)
            {
                //创建行
                IRow cells = sheet1.CreateRow(i + 2);
                for (int j = 0; j < 5; j++)
                {
                    ICell Icell3 = cells.CreateCell(j);

                    //将新的样式赋给单元格
                    Icell3.CellStyle = Istyle3;
                    var test = new object();
                    if (j == 0)
                        test = FinanceList[i].Id;
                    else if (j == 1)
                        test = FinanceList[i].User_Number;
                    else if (j == 2)
                        test = FinanceList[i].Principal;
                    else if (j == 3)
                        test = FinanceList[i].Amount;
                    else
                    {
                        test = FinanceList[i].CreateTime;
                    }

                    Icell3.SetCellValue(test.ToString());

                }

            }

            //根据内容动态调整excel单元格宽
            sheet1.SetColumnWidth(0, 20 * 150);
            sheet1.SetColumnWidth(1, 20 * 250);
            sheet1.SetColumnWidth(2, 20 * 200);
            sheet1.SetColumnWidth(3, 20 * 200);
            sheet1.SetColumnWidth(4, 20 * 350);

            //声明一个存放内存流的容器
            MemoryStream ExcelStream = new MemoryStream();
            //Exce1文件写入内存流
            exce1Book.Write(ExcelStream);

            //Seek (0, Seek. begin)第一-个参数表示相对位置，第二个参数表示参照位置
            ExcelStream.Seek(0, SeekOrigin.Begin);

            //为Exce1文件命名
            string fileName = "流水信息" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
            //MIME文件类型(Multipurpose Internet Mail Extensions)多 用途互联网邮件扩展类型
            return File(ExcelStream, "application/vnd.ms-exce1'", fileName);

        }
    }
}

