using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Model.Entities
{
    [Table("TP_News")]
   public class News : BE
    {
        [StringLength(50)]
        public string New_Title { get; set; }
        public long? New_Class { get; set; }
        public string New_Content { get; set; }
        public bool IsEnable { get; set; }
    }
}
