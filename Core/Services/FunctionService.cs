using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services
{
    public class FunctionService : IFunctionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FunctionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> CheckIfExistedIdAsync(string id) => (await _unitOfWork.Repository<Function, string>().FindByIdAsync(id)) != null;

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public Task<IReadOnlyList<Function>> GetAllByPermissionAsync(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}