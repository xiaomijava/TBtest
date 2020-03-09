namespace TradingPlatform.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using TradingPlatform.Model.BaseContext;
    using TradingPlatform.Model.Entities;

    public partial class TPDbContext : BaseDbContext
    {
        public TPDbContext()
            : base("name=ConnectString")
        {
        }
        public TPDbContext(string connectString)
            : base(connectString)
        {
        }
        public virtual DbSet<Menu> Menu { get; set; }

     
        public virtual DbSet<User> User { get; set; }
        
        public virtual DbSet<Finance> Finance { get; set; }
    
        public virtual DbSet<Role> Role { get; set; }

        public virtual DbSet<News> News { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
