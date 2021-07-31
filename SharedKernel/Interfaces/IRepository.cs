using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface IRepository<T, TId> where T : BaseEntity<TId>
    {
        Task<T> FindByIdAsync(TId id, params Expression<Func<T, object>>[] includeProperties);

        Task<T> FindSingleAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(params Expression<Func<T, object>>[] includeProperties);

        IQueryable<T> FindAll(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(T entity);

        Task DeleteAsync(TId id);

        void DeleteMultipleAsync(IEnumerable<T> entities);

        IQueryable<T> ApplySpecification(ISpecification<T> specification = null);
    }
}