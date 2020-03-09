using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Model
{
    [Table("FinanceInfo")]
    public class Finance:BE
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        public string User_Number { get; set; }
    
        /// <summary>
        /// 类型ID
        /// </summary>
        public int TypeID { get; set; }


        /// <summary>
        /// 金额(系统数字货币)
        /// </summary>
        public decimal? Amount { get; set; }

        /// <summary>
        /// 本金
        /// </summary>
        public decimal Principal { get; set; } = 0;
    }
}
