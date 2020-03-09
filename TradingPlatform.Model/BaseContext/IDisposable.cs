using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingPlatform.Model.BaseContext
{
    public interface IDbContext : IDisposable
    {
        Database Database { get; }

        DbChangeTracker ChangeTracker { get; }

        DbContextConfiguration Configuration { get; }

        DbEntityEntry Entry(object entity);

        DbEntityEntry<TEntity> Entry<TEntity>(TEntity entity) where TEntity : class;

        IEnumerable<DbEntityValidationResult> GetValidationErrors();

        DbSet Set(Type entityType);

        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync();
    }
}
