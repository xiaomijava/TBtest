using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.Model;
using TradingPlatform.Model.Entities;

namespace TradingPlatform.IService
{
    public interface IUserService:IBaseService<User>
    {
        User GetMerchat(string number);
    }
}
