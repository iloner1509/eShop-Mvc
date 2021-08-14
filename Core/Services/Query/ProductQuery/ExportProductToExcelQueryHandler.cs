using System;
using System.IO;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.SharedKernel.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace eShop_Mvc.Core.Services.Query.ProductQuery
{
    public class ExportProductToExcelQueryHandler : IRequestHandler<ExportProductToExcelQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExportProductToExcelQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(ExportProductToExcelQuery request, CancellationToken cancellationToken)
        {
            string directory = Path.Combine(request.WebRootPath, "export-files");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            string fileName = $"DanhSachSanPham_Ngay_{DateTime.Now:ddMMyyyy-hh:mm:ss}.xlsx";
            string fileUrl = $"{request.HttpRequest.Scheme}://{request.HttpRequest.Host}/export-files/{fileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, fileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(request.WebRootPath, fileName));
            }

            var products = await _unitOfWork.Repository<Product, int>().ListAllAsync(cancellationToken);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Danh sách sản phẩm");
                worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Dark8);
                worksheet.Cells.AutoFitColumns();
                await package.SaveAsync(cancellationToken);
            }

            return fileUrl;
        }
    }
}