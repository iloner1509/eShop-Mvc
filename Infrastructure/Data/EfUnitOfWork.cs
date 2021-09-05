using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace eShop_Mvc.Infrastructure.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private Hashtable _repositories;
        private IDbContextTransaction _transaction;

        public EfUnitOfWork(AppDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public int Complete()
        {
            TrackChange();
            return _context.SaveChanges();
        }

        public Task<int> CompleteAsync(CancellationToken cancellationToken)
        {
            TrackChange();
            return _context.SaveChangesAsync(cancellationToken);
        }

        public void BeginTransaction()
        {
            _transaction = _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _transaction.Commit();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public void Rollback()
        {
            _transaction.Rollback();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
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

        private void TrackChange()
        {
            var modified = _context.ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (EntityEntry item in modified)
            {
                if (item.Entity is IAuditable changedOrAddedItem)
                {
                    if (item.State == EntityState.Added)
                    {
                        changedOrAddedItem.CreatedBy = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
                        changedOrAddedItem.DateCreated = DateTime.Now;
                    }
                    changedOrAddedItem.ModifiedBy = _httpContextAccessor?.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
                    changedOrAddedItem.DateModified = DateTime.Now;
                }

                if (item.Entity is IIpTracking changeOrAddedItem)
                    changeOrAddedItem.IpAddress = _httpContextAccessor?.HttpContext?.Connection?.RemoteIpAddress.ToString();
            }
        }
    }
}