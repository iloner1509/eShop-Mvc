using System;
using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IMapper mapper, IWebHostEnvironment hostEnvironment)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Ajax api

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(await _productService.GetAllAsync());
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var model = _mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(await _productCategoryService.GetAllAsync());
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = await _productService.GetAllPagingAsync(categoryId, keyword, page, pageSize);

            return new OkObjectResult(new PagedResult<ProductViewModel>()
            {
                CurrentPage = model.CurrentPage,
                PageSize = model.PageSize,
                RowCount = model.RowCount,
                Results = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(model.Results)
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = _mapper.Map<Product, ProductViewModel>(await _productService.GetByIdAsync(id));
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }

            productViewModel.SeoAlias = TextHelper.ToUnsignString(productViewModel.Name);
            if (productViewModel.Id == 0)
            {
                await _productService.AddAsync(_mapper.Map<ProductViewModel, Product>(productViewModel));
            }
            else
            {
                await _productService.UpdateAsync(_mapper.Map<ProductViewModel, Product>(productViewModel));
            }
            _productService.Save();
            return new OkObjectResult(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _productService.DeleteAsync(id);
            _productService.Save();
            return new OkObjectResult(id);
        }

        [HttpPost]
        public async Task<IActionResult> ImportExcel(IList<IFormFile> files, int categoryId)
        {
            if (files != null && files.Count > 0)
            {
                var file = files[0];
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                string folder = _hostEnvironment.WebRootPath + $@"\uploaded\excels";
                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string filePath = Path.Combine(folder, fileName);
                await using (FileStream fs = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(fs);
                    await fs.FlushAsync();
                }

                await _productService.ImportExcelAsync(filePath, categoryId);
                _productService.Save();
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel()
        {
            string webRootPath = _hostEnvironment.WebRootPath;
            string directory = Path.Combine(webRootPath, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fileName = $"DanhSachSanPham_Ngay_{DateTime.Now:ddMMyyyy-hhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/export-files/{fileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, fileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(webRootPath, fileName));
            }

            var products = await _productService.GetAllAsync();
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Danh sách sản phẩm");
                worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Dark8);
                worksheet.Cells.AutoFitColumns();
                await package.SaveAsync();
            }
            return new OkObjectResult(fileUrl);
        }

        #endregion Ajax api
    }
}