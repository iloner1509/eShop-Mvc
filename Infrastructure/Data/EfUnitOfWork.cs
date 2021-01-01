using eShop_Mvc.SharedKernel.Interfaces;

namespace eShop_Mvc.Infrastructure.Data
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public EfUnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}