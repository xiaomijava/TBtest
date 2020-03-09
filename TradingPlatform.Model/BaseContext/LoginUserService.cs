using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TradingPlatform.Model.BaseContext
{

    public class LoginUserService :  ILoginUserService
    {
        private string LoginName {
            get {
                HttpContext current = HttpContext.Current;
                if (current.Session["SourceName"] != null) {
                    return current.Session["SourceName"].ToString();
                }
                return null;
            }
        }

        public string CreateSource
        {
            get
            {
                return this.LoginName == null ? (string)null : this.LoginName;
            }
        }

        public string UpdateSource
        {
            get
            {
                return this.LoginName == null ? (string)null : this.LoginName;
            }
        }
    }
}
