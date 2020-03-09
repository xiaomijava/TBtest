using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;

namespace TradingPlatform.Model
{
    [Table("MenuInfo")]
    public partial class Menu : BE
    {
        public long? Parent_ID { get; set; }

        [StringLength(20)]
        public string Menu_Name { get; set; }

        [StringLength(255)]
        public string Menu_Path { get; set; }

        [StringLength(255)]
        public string Menu_Icon { get; set; }

        public int ZIndex { get; set; }

    }
}
