using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Model.Entities
{
    [Table("RoleInfo")]
    public partial class Role : BE
    {
        public string Name { get; set; }

        public int Level { get; set; }

        public string Desc { get; set; }

        public string MenuIDs { get; set; }
    }
}
