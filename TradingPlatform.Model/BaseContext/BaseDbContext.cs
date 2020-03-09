using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace TradingPlatform.Model.BaseContext
{
    public class BaseDbContext : DbContext, IDbContext, IDisposable
    {
        public ILoginUserService LoginUserService = new LoginUserService();

        public BaseDbContext()
        {
            this.Configuration.LazyLoadingEnabled = true;
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = new int?(1800);
        }

        public BaseDbContext(string connectionString)
          : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = true;
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = new int?(1800);
        }

        private void Add()
        {
            List<DbEntityEntry> list = this.ChangeTracker.Entries().Where<DbEntityEntry>((Func<DbEntityEntry, bool>)(e => e.State == EntityState.Modified)).ToList<DbEntityEntry>();
            foreach (DbEntityEntry dbEntityEntry in this.ChangeTracker.Entries().Where<DbEntityEntry>((Func<DbEntityEntry, bool>)(e => e.State == EntityState.Added)).ToList<DbEntityEntry>())
            {
                BE entity = dbEntityEntry.Entity as BE;
                if (entity != null)
                {
                    if (!entity.IsDelete.HasValue)
                        entity.IsDelete = new bool?(false);
                    try
                    {
                        if (this.LoginUserService != null)
                        {
                            entity.CreateSource = this.LoginUserService.CreateSource;
                            entity.UpdateSource = this.LoginUserService.UpdateSource;
                        }
                    }
                    catch(Exception ex)
                    {
                        entity.CreateSource = null;
                        entity.UpdateSource = null;
                        //throw new Exception("LoginUserService is null");
                    }
                    DateTime? nullable = entity.CreateTime;
                    if (!nullable.HasValue)
                        entity.CreateTime = new DateTime?(DateTime.Now);
                    nullable = entity.UpdateTime;
                    if (!nullable.HasValue)
                        entity.UpdateTime = new DateTime?(DateTime.Now);
                }
            }
            foreach (DbEntityEntry dbEntityEntry in list)
            {
                BE entity = dbEntityEntry.Entity as BE;
                if (entity != null)
                {
                    try
                    {
                        if (this.LoginUserService != null)
                            entity.UpdateSource = this.LoginUserService.UpdateSource;
                    }
                    catch(Exception ex)
                    {
                        entity.UpdateSource = null;
                    }
                    entity.UpdateTime = new DateTime?(DateTime.Now);
                }
            }
        }

        public override int SaveChanges()
        {
            this.Add();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync()
        {
            this.Add();
            return Task.Run<int>((Func<int>)(() => base.SaveChanges()));
        }

        DbEntityEntry IDbContext.Entry(object entity)
        {
            return this.Entry(entity);
        }

        DbEntityEntry<TEntity> IDbContext.Entry<TEntity>(TEntity entity)
        {
            return this.Entry<TEntity>(entity);
        }

        IEnumerable<DbEntityValidationResult> IDbContext.GetValidationErrors()
        {
            return this.GetValidationErrors();
        }
    }
}
