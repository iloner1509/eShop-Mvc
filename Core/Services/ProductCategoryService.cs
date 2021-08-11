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
    public class ProductCategoryService : IProductCategoryService
    {
        public async Task<IReadOnlyList<ProductCategory>> GetHomeCategoriesAsync(int top)
        {
            //var categories = await _productCategoryRepository.FindAll(x => x.HomeFlag == true, c => c.Products)
            //    .OrderBy(x => x.HomeOrder).Take(top).ToListAsync();

            //return categories;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}