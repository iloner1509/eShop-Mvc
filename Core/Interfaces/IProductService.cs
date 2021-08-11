using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel;

namespace eShop_Mvc.Core.Interfaces
{
    public interface IProductService : IDisposable
    {
        Task<Product> AddAsync(Product product);

        Task UpdateAsync(Product product);

        Task DeleteAsync(int id);

        Task<Product> GetByIdAsync(int id);

        Task ImportExcelAsync(string filePath, int categoryId);

        void Save();

        Task AddImageAsync(int productId, string[] images);

        Task<IReadOnlyList<ProductImage>> GetImagesAsync(int productId);

        Task AddWholePriceAsync(int productId, List<WholePrice> wholePrices);

        Task<IReadOnlyList<WholePrice>> GetWholePricesAsync(int productId);

        Task<IReadOnlyList<Product>> GetLatestAsync(int top);

        Task<IReadOnlyList<Product>> GetHotProductsAsync(int top);

        Task<IReadOnlyList<Product>> GetRelatedProductsAsync(int id, int top);

        Task<IReadOnlyList<Tag>> GetProductTagsAsync(int productId);

        bool CheckAvailability(int productId);

        Task<PagedResult<Product>> GetAllPagingAsync(int? categoryId, string keyword, int page, int pageSize);
    }
}