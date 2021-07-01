using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services
{
    public class FunctionService : IFunctionService
    {
        private readonly IRepository<Function, string> _functionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FunctionService(IRepository<Function, string> functionRepository, IUnitOfWork unitOfWork)
        {
            _functionRepository = functionRepository;
            _unitOfWork = unitOfWork;
        }

        public Task<Function> GetByIdAsync(string id) => _functionRepository.FindByIdAsync(id);

        public async Task<bool> CheckIfExistedIdAsync(string id) => (await _functionRepository.FindByIdAsync(id)) != null;

        public Task AddAsync(Function function) => _functionRepository.AddAsync(function);

        public Task UpdateAsync(Function function) => _functionRepository.UpdateAsync(function);

        public Task DeleteAsync(string id) => _functionRepository.DeleteAsync(id);

        public async Task UpdateParentIdAsync(string sourceId, string targetId, Dictionary<string, int> items)
        {
            var sourceFunction = await _functionRepository.FindByIdAsync(sourceId);
            sourceFunction.ParentId = targetId;
            await _functionRepository.UpdateAsync(sourceFunction);

            var sibling = _functionRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                await _functionRepository.UpdateAsync(child);
            }
        }

        public async Task ReOrderAsync(string sourceId, string targetId)
        {
            var source = await _functionRepository.FindByIdAsync(sourceId);
            var target = await _functionRepository.FindByIdAsync(targetId);

            // Swap
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            await _functionRepository.UpdateAsync(source);
            await _functionRepository.UpdateAsync(target);
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IReadOnlyList<Function>> GetAllAsync(string filter)
        {
            var query = _functionRepository.FindAll(x => x.Status == Status.Active);
            if (!string.IsNullOrEmpty(filter))
            {
                query = query.Where(x => x.Name.Contains(filter));
            }

            return await query.OrderBy(x => x.ParentId).ToListAsync();
        }

        public Task<IReadOnlyList<Function>> GetAllByPermissionAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}