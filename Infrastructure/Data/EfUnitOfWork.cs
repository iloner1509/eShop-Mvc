using System;
using System.Collections;
using System.Threading;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using System.Threading.Tasks;

namespace eShop_Mvc.Infrastructure.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable _repositories;

        public EfUnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : BaseEntity<TId>
        {
            _repositories ??= new Hashtable();

            var type = typeof(TEntity).Name;
            if (!_repositories.Contains(type))
            {
                var repoType = typeof(EfRepository<,>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity), typeof(TId)), _context);
                _repositories.Add(type, repoInstance);
            }

            return (IRepository<TEntity, TId>)_repositories[type];
        }
    }
}