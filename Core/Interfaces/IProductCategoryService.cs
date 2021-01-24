using System;
using eShop_Mvc.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IProductCategoryService : IDisposable
    {
        Task<ProductCategory> AddAsync(ProductCategory productCategory);

        Task UpdateAsync(ProductCategory productCategory);

        Task DeleteAsync(int id);

        Task<IReadOnlyList<ProductCategory>> GetAllAsync();

        Task<IReadOnlyList<ProductCategory>> GetAllAsync(string keyword);

        Task<IReadOnlyList<ProductCategory>> GetAllByParentIdAsync(int parentId);

        Task<ProductCategory> GetByIdAsync(int id);

        Task UpdateParentIdAsync(int sourceId, int targetId, Dictionary<int, int> items);

        Task ReOrderAsync(int sourceId, int targetId);

        Task<IReadOnlyList<ProductCategory>> GetHomeCategoriesAsync(int top);

        void Save();
    }
}