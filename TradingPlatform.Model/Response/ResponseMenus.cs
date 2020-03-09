

using System.Collections.Generic;

namespace TradingPlatform.Model
{
    public class ResponseMenus
    {
        public Menu menu { get; set; }
        public List<Menu> sonmenus { get; set; }
        public bool key { get; set; }
    }
}