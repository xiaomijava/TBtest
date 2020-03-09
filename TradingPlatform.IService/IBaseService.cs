using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TradingPlatform.IService.Enum;
using TradingPlatform.Model;

namespace TradingPlatform.IService
{
    public interface IBaseService<T> where T : BE, new()
    {
        /// <summary>
        /// Gets the by id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>`0.</returns>
        T GetById(object id);
        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        int Insert(T entity);

        int Insert(IEnumerable<T> entities);
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        int Update(T entity);
        /// <summary>
        /// Updates the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        int Update(IEnumerable<T> entity);
        /// <summary>
        /// Deletes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        int Delete(T entity);
        /// <summary>
        /// Deletes the specified entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        int Delete(IEnumerable<T> entities);
        /// <summary>
        /// Deletes the specified entity .
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="deleteType">Delete type</param>
        /// <returns></returns>
        int Delete(T entity, EnumDeleteType deleteType = EnumDeleteType.Logically);

        /// <summary>
        /// Deletes the specified entities .
        /// </summary>
        /// <param name="entity">The entities.</param>
        /// <param name="deleteType">Delete type</param>
        /// <returns></returns>
        int Delete(IEnumerable<T> entities, EnumDeleteType deleteType = EnumDeleteType.Logically);
        /// <summary>
        /// Gets the table.
        /// </summary>
        /// <value>The table.</value>
        IQueryable<T> Table { get; }
        /// <summary>
        /// Gets the tables no tracking.
        /// </summary>
        /// <value>The tables no tracking.</value>
        IQueryable<T> TableNoTracking { get; }

        IQueryable<T> LoadPageItems<Tkey>(int pageSize, int pageIndex, out int total, Expression<Func<T, bool>> whereLambda,Func<T, Tkey> orderbyLambda, bool isAsc = false);

        IQueryable<T> GetList(Expression<Func<T, bool>> whereLambda);

        IQueryable<T> GetList<Tkey>(Expression<Func<T, bool>> whereLambda, Func<T, Tkey> orderbyLambda, bool isAsc = false);
    }
}
