using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Model.Entities
{

    [Table("UserInfo")]
    public partial class User : BE
    {
        public string User_Name { get; set; }

        public string User_Number { get; set; }

        public string RoleIds { get; set; }
        public string U_1st_Password { get; set; }

        public string U_2st_Password { get; set; }

        public string U_3st_Password { get; set; }

        public decimal? Money { get; set; }

        public decimal? OneMoney { get; set; }

        public string User_Url { get; set; }
        public string Bindphone { get; set; }
        public string U_Passport { get; set; }

        public int? User_Sex { get; set; }

        public string User_NickName { get; set; }

        public string User_QQ { get; set; }

        public string User_Email { get; set; }


    }

}
