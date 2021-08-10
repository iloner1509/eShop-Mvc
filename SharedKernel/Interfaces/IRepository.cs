using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface IRepository<T, TId> where T : BaseEntity<TId>
    {
        Task<T> FindByIdAsync(TId id);

        Task AddAsync(T entity, CancellationToken cancellationToken);

        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken);

        void Update(T entity);

        void Delete(T entity);

        Task DeleteByIdAsync(TId id);

        void DeleteRange(IEnumerable<T> entities);

        Task<T> FindSingleAsync(CancellationToken cancellationToken, ISpecification<T> specification = null);

        Task<T> FindFirstAsync(CancellationToken cancellationToken, ISpecification<T> specification = null);

        Task<IReadOnlyList<T>> FindAllAsync(CancellationToken cancellationToken, ISpecification<T> specification = null);

        Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken);

        Task<bool> ContainsAsync(CancellationToken cancellationToken, ISpecification<T> specification = null);

        Task<bool> ContainsAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate);

        Task<int> CountAsync(CancellationToken cancellationToken, ISpecification<T> specification = null);

        Task<int> CountAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate);

        IQueryable<T> ApplySpecification(ISpecification<T> specification = null);
    }
}