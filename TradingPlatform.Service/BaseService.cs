using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using TradingPlatform.IService;
using TradingPlatform.IService.Enum;
using TradingPlatform.Model;

namespace TradingPlatform.Service
{
    public class BaseService<T> : IBaseService<T> where T : BE,new()
    {
        #region Fields

        private readonly Model.TPDbContext _context = EFContextFactory.GetCurrentDbContext();

        private IDbSet<T> _entities;
     
        #endregion
        public BaseService()
        {
        }
        #region property

        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _context.Set<T>();
                }
                return _entities ?? (_entities = _context.Set<T>());
                // _entities ?? _entities = db.Set<T>();
            }
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }

        public IQueryable<T> TableNoTracking
        {
            get
            {
                return Entities.AsNoTracking();

            }
        }


        #endregion



        public virtual T GetById(object id)
        {
            return Entities.Find(id);
        }

        public int Insert(T entity)
        {
            if (entity == null) throw new ArgumentException("entity");
            Entities.Add(entity);
            //try {

            //}
            //catch (DbUpdateConcurrencyException ex) {
            //    //TimeStamp
            //}
            return _context.SaveChanges();
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual int Insert(IEnumerable<T> entities)
        {

            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
                Entities.Add(entity);

            return _context.SaveChanges();
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            using (Model.TPDbContext ef = new Model.TPDbContext())
            {
                var entitys = ef.Entry(entity);
                entitys.State = EntityState.Modified;
                return ef.SaveChanges();
            }
           
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int Update(IEnumerable<T> entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            using (Model.TPDbContext ef = new Model.TPDbContext())
            {
                foreach (var item in entity)
                {
                    var entitys = ef.Entry(item);
                    entitys.State = EntityState.Modified;
                }
              
                return ef.SaveChanges();
            }

        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");

            entity.IsDelete = true;

            return _context.SaveChanges();
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual int Delete(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");

            foreach (var entity in entities)
            {
                entity.IsDelete = true;
                var entitys = _context.Entry(entity);
                entitys.State = EntityState.Modified;
            }
            return _context.SaveChanges();
        }
        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual int Delete(T entity, EnumDeleteType deleteType = EnumDeleteType.Logically)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            if (deleteType == EnumDeleteType.Physically)
            {
                Entities.Remove(entity);
            }
            else {
                entity.IsDelete = true;
            }

            return _context.SaveChanges();
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual int Delete(IEnumerable<T> entities, EnumDeleteType deleteType = EnumDeleteType.Logically)
        {
            if (entities == null)
                throw new ArgumentNullException("entities");
            if (deleteType == EnumDeleteType.Physically)
            {
                foreach (var entity in entities)
                    Entities.Remove(entity);
            }
            else {
                foreach (var entity in entities) {
                    entity.IsDelete = true;
                    var entitys = _context.Entry(entity);
                    entitys.State = EntityState.Modified;
                }
            }
            return _context.SaveChanges();
        }

        public int Count()
        {
            return this._entities.Count<T>();
        }
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="total"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderName"></param>
        /// <param name="isAsc"></param>
        /// <returns></returns>
        public IQueryable<T> LoadPageItems<Tkey>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda,Func<T, Tkey> orderbyLambda, bool isAsc=false)
        {
            //var type = typeof(T);
            //var propertyName = orderName;
            //var param = Expression.Parameter(type, type.Name);
            //var body = Expression.Property(param, propertyName);
            //var keySelector = Expression.Lambda(body, param);
            //Expression<Func<T, DateTime>> orderbyLambda = c =>keySelector.Body;
            //Func<T, DateTime> orderbyLambda = keySelector.Compile() as Func<T, DateTime>;
            //Expression<Func<T, DateTime>> orderbyLambda = keySelector as Expression<Func<T, DateTime>>;
            total = _context.Set<T>().Where(whereLambda).Count();
            if (isAsc)
            {
                var temp = _context.Set<T>().Where(whereLambda)
                             .OrderBy<T, Tkey>(orderbyLambda)
                             //.OrderBy(orderbyLambda)
                             .Skip(pageSize * (pageIndex - 1))
                             .Take(pageSize);
                return temp.AsQueryable();
            }
            else
            {
                var temp = _context.Set<T>().Where(whereLambda)
                           .OrderByDescending<T, Tkey>(orderbyLambda)
                           .Skip(pageSize * (pageIndex - 1))
                           .Take(pageSize);
                return temp.AsQueryable();
            }
        }

        /// <summary>
        /// 按条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda) { 

                var temp = _context.Set<T>().Where(whereLambda);
                return temp.AsQueryable();
        }

        public IQueryable<T> GetList<Tkey>(Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, bool isAsc = false) {
            if (isAsc)
            {
                var temp = _context.Set<T>().Where(whereLambda)
                             .OrderBy<T, Tkey>(orderbyLambda);
                return temp.AsQueryable();
            }
            else
            {
                var temp = _context.Set<T>().Where(whereLambda)
                           .OrderByDescending<T, Tkey>(orderbyLambda);
                return temp.AsQueryable();
            }
        }
    }
}
