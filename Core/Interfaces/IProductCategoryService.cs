using System;
using eShop_Mvc.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IProductCategoryService : IDisposable
    {
        Task UpdateParentIdAsync(int sourceId, int targetId, Dictionary<int, int> items);

        Task ReOrderAsync(int sourceId, int targetId);

        Task<IReadOnlyList<ProductCategory>> GetHomeCategoriesAsync(int top);
    }
}