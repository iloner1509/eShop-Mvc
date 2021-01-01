using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface IRepository<T, K> where T : BaseEntity<K>
    {
        T FindById(K id, params Expression<Func<T, object>>[] includeProperties);

        T FindSingle(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        void Add(T entity);

        void Update(T entity);

        void Delete(T entity);

        void Delete(K id);

        void DeleteMultiple(IEnumerable<T> entities);
    }
}