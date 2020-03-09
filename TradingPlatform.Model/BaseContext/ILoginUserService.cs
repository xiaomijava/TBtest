using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Model.BaseContext
{
    public interface ILoginUserService
    {
        string CreateSource { get; }

        string UpdateSource { get; }
    }
}
