using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.IService;
using TradingPlatform.Model;

namespace TradingPlatform.Service.Service
{
    public class FinanceService: BaseService<Finance>, IFinanceService
    {
        private TPDbContext _dbContext = EFContextFactory.GetCurrentDbContext();
    }
}
