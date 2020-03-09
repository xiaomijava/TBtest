using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TradingPlatform.IService;
using TradingPlatform.Model;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    public class MemberFinanceController : ControllerOldMemberBase
    {
        IFinanceService _financeService = new FinanceService();
        IFinanceTypeService _financeTypeService = new FinanceTypeService();
        public ActionResult FinanceIndex()
        {
            return View();
        }

        /// <summary>
        /// 获取财务流水数据
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("MemberFinance/GetFinanceData")]
        public JsonResult GetFinanceData(PageParam param)
        {
            int total = 0;
            IQueryable<Finance> finances = _financeService.Table;
            IQueryable<FinanceType> financeTypes = _financeTypeService.Table;
            var list = finances.Join(financeTypes, a => a.TypeID, b => b.Id, (a, b) => new
            {
                IsDelete = a.IsDelete,
                CreateTime = a.CreateTime,
                CreateSource = a.CreateSource,
                UserID = a.User_ID,
                TypeID = b.Id,
                Subject = a.Subject,
                Amount = a.Amount,
                ChangeContent = b.Type_Name,
                C_ID = b.C_ID,
                N_Type = b.N_Type,
                N_Flag = b.N_Flag
            }).Where(s => s.IsDelete == false).ToList();

            total = list.Count();


            var result = list.OrderByDescending(t => t.CreateTime).Skip(param.pageSize * (param.pageIndex - 1)).Take(param.pageSize).ToList();
            return Json(new { table = result, total = total }, JsonRequestBehavior.AllowGet);
        }
    }
}
