using System;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : BaseEntity<TId>;

        int Complete();

        Task<int> CompleteAsync(CancellationToken cancellationToken);
    }
}