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
        private readonly IRepository<ProductCategory, int> _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IRepository<ProductCategory, int> productCategoryRepository, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ProductCategory> AddAsync(ProductCategory productCategory)
        {
            await _productCategoryRepository.AddAsync(productCategory);
            return productCategory;
        }

        public Task UpdateAsync(ProductCategory productCategory) => _productCategoryRepository.UpdateAsync(productCategory);

        public Task DeleteAsync(int id) => _productCategoryRepository.DeleteAsync(id);

        public async Task<IReadOnlyList<ProductCategory>> GetAllAsync() => await _productCategoryRepository.FindAll().OrderBy(x => x.ParentId).ToListAsync();

        public async Task<IReadOnlyList<ProductCategory>> GetAllAsync(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
            {
                return await _productCategoryRepository.FindAll(x => x.Name.Contains(keyword) || x.Description.Contains(keyword)).OrderBy(x => x.ParentId)
                    .ToListAsync();
            }
            else
            {
                return await GetAllAsync();
            }
        }

        public async Task<IReadOnlyList<ProductCategory>> GetAllByParentIdAsync(int parentId)
            => await _productCategoryRepository.FindAll(x => x.Status == Status.Active && x.ParentId == parentId)
                .ToListAsync();

        public Task<ProductCategory> GetByIdAsync(int id) => _productCategoryRepository.FindByIdAsync(id);

        public async Task UpdateParentIdAsync(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var sourceCategory = await _productCategoryRepository.FindByIdAsync(sourceId);
            sourceCategory.ParentId = targetId;
            await _productCategoryRepository.UpdateAsync(sourceCategory);

            // get sibling
            //var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id)).ToList();

            var sibling = _productCategoryRepository.FindAll(x => items.ContainsKey(x.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                await _productCategoryRepository.UpdateAsync(child);
            }
        }

        public async Task ReOrderAsync(int sourceId, int targetId)
        {
            var source = await _productCategoryRepository.FindByIdAsync(sourceId);
            var target = await _productCategoryRepository.FindByIdAsync(targetId);

            // Swap
            int tempOrder = source.SortOrder;
            source.SortOrder = target.SortOrder;
            target.SortOrder = tempOrder;

            await _productCategoryRepository.UpdateAsync(source);
            await _productCategoryRepository.UpdateAsync(target);
        }

        public async Task<IReadOnlyList<ProductCategory>> GetHomeCategoriesAsync(int top)
        {
            var categories = await _productCategoryRepository.FindAll(x => x.HomeFlag == true, c => c.Products)
                .OrderBy(x => x.HomeOrder).Take(top).ToListAsync();

            return categories;
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}