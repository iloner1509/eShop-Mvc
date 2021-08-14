using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Services.Command.ProductCommand;
using eShop_Mvc.Core.Services.Query.CategoryQuery;
using eShop_Mvc.Core.Services.Query.ProductQuery;
using eShop_Mvc.Core.Specifications.ProductSpecification;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IMediator _mediator;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMapper mapper, IWebHostEnvironment hostEnvironment, IMediator mediator, ILogger<ProductController> logger)
        {
            _mapper = mapper;
            _hostEnvironment = hostEnvironment;
            _mediator = mediator;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Ajax api

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _mediator.Send(new GetAllProductQuery());
            return new OkObjectResult(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductViewModel>>(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var model = await _mediator.Send(new GetAllCategoryQuery());
            return new OkObjectResult(_mapper.Map<IReadOnlyList<ProductCategory>, IReadOnlyList<ProductCategoryViewModel>>(model));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(ProductPagingParams pagingParams)
        {
            var model = await _mediator.Send(new GetAllProductPagingQuery()
            {
                PagingParams = pagingParams
            });

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
            var model = await _mediator.Send(new GetProductByIdQuery()
            {
                ProductId = id
            });
            return new OkObjectResult(_mapper.Map<Product, ProductViewModel>(model));
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
                await _mediator.Send(new CreateProductCommand()
                {
                    Product = _mapper.Map<ProductViewModel, Product>(productViewModel)
                });
                _logger.LogInformation("New product had been created !");
            }
            else
            {
                await _mediator.Send(new UpdateProductCommand()
                {
                    Product = _mapper.Map<ProductViewModel, Product>(productViewModel)
                });
                _logger.LogInformation($"Product id:{productViewModel.Id} had been modified !");
            }

            return new OkObjectResult(productViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);
            }

            await _mediator.Send(new DeleteProductCommand()
            {
                ProductId = id
            });

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

                await _mediator.Send(new ImportProductByExcelFileCommand()
                {
                    CategoryId = categoryId,
                    FilePath = filePath
                });
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel()
        {
            return new OkObjectResult(await _mediator.Send(new ExportProductToExcelQuery()
            {
                HttpRequest = Request,
                WebRootPath = _hostEnvironment.WebRootPath
            }));
        }

        #endregion Ajax api
    }
}