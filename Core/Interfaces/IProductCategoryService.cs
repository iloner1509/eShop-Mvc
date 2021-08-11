using System;
using eShop_Mvc.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IProductCategoryService : IDisposable
    {
        Task<IReadOnlyList<ProductCategory>> GetHomeCategoriesAsync(int top);
    }
}