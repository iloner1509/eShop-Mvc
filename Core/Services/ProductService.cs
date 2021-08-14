using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Tag, string> _tagRepository;
        private readonly IRepository<ProductTag, int> _productTagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IRepository<Product, int> productRepository, IRepository<Tag, string> tagRepository,
                              IRepository<ProductTag, int> productTagRepository,
                              IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;

            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }


        public Task AddWholePriceAsync(int productId, List<WholePrice> wholePrices)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<WholePrice>> GetWholePricesAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<Product>> GetHotProductsAsync(int top)
        {
            return await _productRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true)
                .OrderByDescending(x => x.DateCreated).Take(top).ToListAsync();
        }

        public async Task<IReadOnlyList<Product>> GetRelatedProductsAsync(int id, int top)
        {
            var product = await _productRepository.FindByIdAsync(id);
            return await _productRepository
                .FindAll(x => x.Id != id && x.Status == Status.Active && x.CategoryId == product.CategoryId)
                .OrderByDescending(x => x.DateCreated).Take(top).ToListAsync();
        }

        public async Task<IReadOnlyList<Tag>> GetProductTagsAsync(int productId)
        {
            var tags = _tagRepository.FindAll();
            var productTags = _productTagRepository.FindAll();
            var query = from t in tags
                        join pt in productTags on t.Id equals pt.TagId
                        where pt.ProductId == productId
                        select new Tag()
                        {
                            Id = t.Id,
                            Name = t.Name
                        };
            return await query.ToListAsync();
        }

        public bool CheckAvailability(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<Product>> GetAllPagingAsync(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll(x => x.ProductCategory);
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId);
            }

            int totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);

            var data = await query.ToListAsync();
            return new PagedResult<Product>()
            {
                CurrentPage = page,
                PageSize = pageSize,
                RowCount = totalRow,
                Results = data
            };
        }
    }
}