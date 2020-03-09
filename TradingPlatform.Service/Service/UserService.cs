using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.IService;
using TradingPlatform.Model;
using TradingPlatform.Model.Entities;

namespace TradingPlatform.Service
{
    public class UserService:BaseService<User>,IUserService
    {
        private TPDbContext _dbContext = EFContextFactory.GetCurrentDbContext();

        /// <summary>
        /// 根据会员编号判断是否为承兑商
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public User GetMerchat(string number)
        {
            User user = new User();
            if (string.IsNullOrEmpty(number))
            {
                user = null;
                return user;
            }
            TPDbContext _dbContext1 = new TPDbContext();
            try
            {
                user = _dbContext.User.FirstOrDefault(u => u.IsDelete == false && u.User_Number == number );
            }
            catch
            {
                user = null;
            }
            return user;
        }
    }
}
