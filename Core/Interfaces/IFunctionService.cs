using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        Task<IReadOnlyList<Function>> GetAllByPermissionAsync(Guid userId);

        Task<bool> CheckIfExistedIdAsync(string id);
    }
}