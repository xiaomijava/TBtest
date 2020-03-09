using TradingPlatform.IService.IService;
using TradingPlatform.Model;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace TradingPlatform.Service.Service
{
    public class MenuService : BaseService<Menu>, IMenuService
    {
        private TPDbContext _dbContext = EFContextFactory.GetCurrentDbContext();
        /// <summary>
        /// 获取菜单，目前只支持二级菜单，
        /// </summary>
        /// <param name="AdminId"></param>
        /// <returns></returns>
        public List<MenuModel> GetMenueList(long AdminId)
        {
            //根据管理员获取对应的角色
            var roles = _dbContext.User.Where(t => t.Id == AdminId && t.IsDelete == false).Select(t => t.RoleIds).FirstOrDefault();

            var menuIds = roles.Split(',');

            //获取管理员对应角色的菜单
            var rolesMenus = (
                         from a in _dbContext.Role.Where(t => t.IsDelete == false)
                         where menuIds.Contains(a.Id.ToString())
                         select new
                         {
                             a.MenuIDs,
                         }
                ).Select(o => o.MenuIDs).Distinct().ToArray();

            //当前用户所拥有权限菜单转化为数组并去重

            var rolesMenusall = string.Join(",", rolesMenus).Split(',').GroupBy(t=>t).Select(t=>t.Key).ToArray(); 
          
            var result = (from a in _dbContext.Menu.Where(t => t.IsDelete == false)
                          where rolesMenusall.Contains(a.Id.ToString()) && a.Parent_ID == 0
                          orderby a.ZIndex
                          select new MenuModel
                          {
                              FirstMenu = a,
                              SecondMenus = _dbContext.Menu.Where(o => o.IsDelete == false && o.Parent_ID == a.Id && rolesMenusall.Contains(o.Id.ToString())).OrderBy(o => o.ZIndex).ToList()
                          }
                ).ToList();
            return result;
        }
    }
}
