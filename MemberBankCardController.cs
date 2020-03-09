using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using TradingPlatform.Common;
using TradingPlatform.IService;
using TradingPlatform.IService.IService;
using TradingPlatform.Model;
using TradingPlatform.Service;
using TradingPlatform.Service.Service;

namespace TradingPlatform.Controllers
{
    public class MemberBankCardController : ControllerOldMemberBase
    {
        IBankCardService  bankCardService = new BankCardService();
        
        /// <summary>
        /// 银行卡信息页面
        /// </summary>
        /// <returns></returns>
        public ActionResult BankIndex()
        {
          
            return View();
        }


        /// <summary>
        /// 获取分页菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("MemberBankCard/GetNewseByPage")]
        public JsonResult GetNewseByPage(PageParam param)
        {
            int total = 0;
            ResponseList<BankCard> response = new ResponseList<BankCard>();
            Expression<Func<BankCard, bool>> where = c => true;
            where = where.And(m => m.IsDelete == false);
            List<BankCard> menus = new List<BankCard>();
            menus = bankCardService.LoadPageItems<DateTime?>(param.pageSize, param.pageIndex, out total, where, c => c.CreateTime, param.IsAsc).ToList();
            response.result = true;
            response.data = menus;
            return Json(new { responseData = response, total = total }, JsonRequestBehavior.AllowGet);
        }
    }
}
