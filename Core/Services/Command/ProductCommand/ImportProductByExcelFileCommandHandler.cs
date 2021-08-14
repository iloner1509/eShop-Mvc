using System;
using System.IO;
using System.Security.Claims;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using OfficeOpenXml;

namespace eShop_Mvc.Core.Services.Command.ProductCommand
{
    public class ImportProductByExcelFileCommandHandler : IRequestHandler<ImportProductByExcelFileCommand>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;

        public ImportProductByExcelFileCommandHandler(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }

        public async Task<Unit> Handle(ImportProductByExcelFileCommand request, CancellationToken cancellationToken)
        {
            using var package = new ExcelPackage(new FileInfo(request.FilePath));
            ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            for (var i = worksheet.Dimension.Start.Row + 1; i <= worksheet.Dimension.End.Row; i++)
            {
                var product = new Product();
                product.CategoryId = request.CategoryId;
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
                product.CreatedBy = _userService.GetUser().FindFirstValue(claimType: ClaimTypes.Name);

                await _unitOfWork.Repository<Product, int>().AddAsync(product, cancellationToken);
            }
            await _unitOfWork.CompleteAsync(cancellationToken);
            return Unit.Value;
        }
    }
}