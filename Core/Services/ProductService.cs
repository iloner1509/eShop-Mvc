using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel.Enums;

namespace eShop_Mvc.Core.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IRepository<Tag, string> _tagRepository;
        private readonly IRepository<ProductTag, int> _productTagRepository;
        private readonly IRepository<ProductImage, int> _productImageRepository;
        private readonly IRepository<WholePrice, int> _wholePriceRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IRepository<Product, int> productRepository, IRepository<Tag, string> tagRepository, IRepository<ProductTag, int> productTagRepository,
            IRepository<ProductImage, int> productImageRepository, IRepository<WholePrice, int> wholePriceRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
            _productImageRepository = productImageRepository;
            _wholePriceRepository = wholePriceRepository;
            _unitOfWork = unitOfWork;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync() => await _productRepository.FindAll(x => x.ProductCategory).ToListAsync();

        public async Task<Product> AddAsync(Product product)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] tags = product.Tags.Split(",");
                foreach (var tag in tags)
                {
                    var tagId = TextHelper.ToUnsignString(tag);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag t = new Tag
                        {
                            Id = tagId,
                            Name = tag,
                            Type = "Product"
                        };
                        await _tagRepository.AddAsync(t);
                    }
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }

                foreach (var productTag in productTags)
                {
                    product.ProductTags.Add(productTag);
                }
            }
            await _productRepository.AddAsync(product);
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            List<ProductTag> productTags = new List<ProductTag>();
            if (!string.IsNullOrEmpty(product.Tags))
            {
                string[] tags = product.Tags.Split(",");
                foreach (var tag in tags)
                {
                    var tagId = TextHelper.ToUnsignString(tag);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag t = new Tag
                        {
                            Id = tagId,
                            Name = tag,
                            Type = "Product"
                        };
                        await _tagRepository.AddAsync(t);
                    }

                    _productTagRepository.DeleteMultipleAsync(_productTagRepository.FindAll(x => x.Id == product.Id)
                        .ToList());
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    productTags.Add(productTag);
                }

                foreach (var productTag in productTags)
                {
                    product.ProductTags.Add(productTag);
                }
            }
            await _productRepository.UpdateAsync(product);
        }

        public Task DeleteAsync(int id) => _productRepository.DeleteAsync(id);

        public Task<Product> GetByIdAsync(int id) => _productRepository.FindByIdAsync(id);

        public async Task ImportExcelAsync(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                Product product;
                for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
                {
                    product = new Product();
                    product.CategoryId = categoryId;
                    product.Name = worksheet.Cells[i, 1].Value.ToString();
                    product.Description = worksheet.Cells[i, 2].Value.ToString();
                    decimal.TryParse(worksheet.Cells[i, 3].Value.ToString(), out var originalPrice);
                    product.OriginalPrice = originalPrice;
                    decimal.TryParse(worksheet.Cells[i, 4].Value.ToString(), out var price);
                    product.Price = price;
                    decimal.TryParse(worksheet.Cells[i, 5].Value.ToString(), out var promotionPrice);
                    product.PromotionPrice = promotionPrice;
                    product.Content = worksheet.Cells[i, 6].Value.ToString();
                    product.SeoKeywords = worksheet.Cells[i, 7].Value.ToString();
                    product.SeoDescription = worksheet.Cells[i, 8].Value.ToString();
                    bool.TryParse(worksheet.Cells[i, 9].Value.ToString(), out var hotFlag);
                    product.HotFlag = hotFlag;
                    bool.TryParse(worksheet.Cells[i, 10].Value.ToString(), out var homeFlag);
                    product.HomeFlag = homeFlag;
                    product.Status = Status.Active;
                    product.DateCreated = DateTime.Now;
                    product.Image = "";
                    await _productRepository.AddAsync(product);
                }
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public Task AddImageAsync(int productId, string[] images)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ProductImage>> GetImagesAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task AddWholePriceAsync(int productId, List<WholePrice> wholePrices)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<WholePrice>> GetWholePricesAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetLatestAsync(int top)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetHotProductsAsync(int top)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetRelatedProductsAsync(int id, int top)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Product>> GetUpSellProductsAsync(int top)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Tag>> GetProductTagsAsync(int productId)
        {
            throw new NotImplementedException();
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