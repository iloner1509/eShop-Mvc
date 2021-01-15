using System;

namespace eShop_Mvc.SharedKernel.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}