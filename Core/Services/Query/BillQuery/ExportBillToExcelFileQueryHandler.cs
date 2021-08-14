using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Specifications.BillSpecification;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services.Query.BillQuery
{
    public class ExportBillToExcelFileQueryHandler : IRequestHandler<ExportBillToExcelFileQuery, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ExportBillToExcelFileQueryHandler(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostingEnvironment)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task<string> Handle(ExportBillToExcelFileQuery request, CancellationToken cancellationToken)
        {
            string webRootFolder = _hostingEnvironment.WebRootPath;
            string fileName = $"Bill_{request.BillId}.xlsx";

            // template
            string templateDoc = Path.Combine(webRootFolder, "template", "BillTemplate.xlsx");
            string url = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host.Value}/export-files/{fileName}";
            FileInfo file = new FileInfo(Path.Combine(webRootFolder, "export-files", fileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(webRootFolder, fileName));
            }

            await using (FileStream templateDocumentStream = File.OpenRead(templateDoc))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    // load order
                    var bill = await _unitOfWork.Repository<Bill, int>().FindByIdAsync(request.BillId);

                    // add new worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[$"Order {bill.Id}"];
                    // insert customer data into template
                    worksheet.Cells[4, 1].Value = "Tên khách hàng: " + bill.CustomerName;
                    worksheet.Cells[5, 1].Value = "Địa chỉ: " + bill.CustomerAddress;
                    worksheet.Cells[6, 1].Value = "SĐT: " + bill.CustomerMobile;

                    int rowIndex = 9;

                    // get order details
                    var specification = new BillDetailsByBillIdSpecification(request.BillId);
                    var billDetails = await _unitOfWork.Repository<BillDetail, int>().FindAllAsync(cancellationToken, specification);
                    int count = 1;
                    foreach (var detail in billDetails)
                    {
                        // cell 1: stt
                        worksheet.Cells[rowIndex, 1].Value = count.ToString();
                        // cell 2: tên sp
                        worksheet.Cells[rowIndex, 2].Value = detail.Product.Name;
                        // cell 3: số lượng
                        worksheet.Cells[rowIndex, 3].Value = detail.Quantity.ToString();
                        // cell 4: giá
                        worksheet.Cells[rowIndex, 4].Value = detail.Price.ToString("N0");
                        // cell 5: tổng giá
                        worksheet.Cells[rowIndex, 5].Value = (detail.Price * detail.Quantity).ToString("N0");

                        rowIndex++;
                        count++;
                    }

                    decimal total = billDetails.Sum(x => x.Quantity * x.Price);
                    //worksheet.Cells[rowIndex + orderDetails.Count + 1, 5].Value = total.ToString("N0");
                    worksheet.Cells[24, 5].Value = total.ToString("N0");
                    var numberWord = "Tổng tiền bằng chữ : " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;
                    worksheet.Cells[28, 3].Value = bill.DateCreated.Day + "," + bill.DateCreated.Month +
                                                   "," + bill.DateCreated.Year;
                    await package.SaveAsAsync(file, cancellationToken);
                }
            }

            return url;
        }
    }
}