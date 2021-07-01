using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IFunctionService : IDisposable
    {
        Task<IReadOnlyList<Function>> GetAllAsync(string filter);

        Task<IReadOnlyList<Function>> GetAllByPermissionAsync(Guid userId);

        Task<Function> GetByIdAsync(string id);

        Task<bool> CheckIfExistedIdAsync(string id);

        Task AddAsync(Function function);

        Task UpdateAsync(Function function);

        Task DeleteAsync(string id);

        Task UpdateParentIdAsync(string sourceId, string targetId, Dictionary<string, int> items);

        Task ReOrderAsync(string sourceId, string targetId);

        void Save();
    }
}