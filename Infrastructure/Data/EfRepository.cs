using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Infrastructure.Data
{
    public class EfRepository<T, TId> : IRepository<T, TId> where T : BaseEntity<TId>
    {
        private readonly AppDbContext _context;

        public EfRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T> FindByIdAsync(TId id)
            => await _context.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
            => await _context.Set<T>().ToListAsync(cancellationToken);

        public async Task AddAsync(T entity, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
            //await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await _context.Set<T>().AddRangeAsync(entities, cancellationToken);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            //return _context.SaveChangesAsync();
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task DeleteByIdAsync(TId id)
        {
            var entity = await FindByIdAsync(id);
            Delete(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public async Task<T> FindSingleAsync(CancellationToken cancellationToken, ISpecification<T> specification = null)
            => await ApplySpecification(specification).SingleOrDefaultAsync(cancellationToken);

        public async Task<T> FindFirstAsync(CancellationToken cancellationToken, ISpecification<T> specification = null)
           => await ApplySpecification(specification).FirstOrDefaultAsync(cancellationToken);

        public async Task<IReadOnlyList<T>> FindAllAsync(CancellationToken cancellationToken, ISpecification<T> specification = null)
            => await ApplySpecification(specification).ToListAsync(cancellationToken);

        public async Task<bool> ContainsAsync(CancellationToken cancellationToken, ISpecification<T> specification = null)
            => await CountAsync(cancellationToken, specification) > 0;

        public async Task<bool> ContainsAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate)
            => await CountAsync(cancellationToken, predicate) > 0;

        public async Task<int> CountAsync(CancellationToken cancellationToken, ISpecification<T> specification = null)
            => await ApplySpecification(specification).CountAsync(cancellationToken);

        public async Task<int> CountAsync(CancellationToken cancellationToken, Expression<Func<T, bool>> predicate)
            => await _context.Set<T>().Where(predicate).CountAsync(cancellationToken);

        public IQueryable<T> ApplySpecification(ISpecification<T> specification = null)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), specification);
        }
    }
}