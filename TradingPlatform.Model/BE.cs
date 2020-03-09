using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Model
{
    public class BE
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public long Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public string CreateSource { get; set; }
        public DateTime? UpdateTime { get; set; }
        public string UpdateSource { get; set; }
        public bool? IsDelete { get; set; }
    }
}
