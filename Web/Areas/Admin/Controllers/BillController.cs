using AutoMapper;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Models.Common;
using eShop_Mvc.Models.ProductViewModels;
using eShop_Mvc.SharedKernel.Enums;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.SharedKernel;
using OfficeOpenXml;

namespace eShop_Mvc.Areas.Admin.Controllers
{
    public class BillController : BaseController
    {
        private readonly IBillService _billService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public BillController(IBillService billService, IWebHostEnvironment hostingEnvironment, IMapper mapper)
        {
            _billService = billService;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var model = await _billService.GetDetailAsync(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPaging(string startDate, string endDate, string keyword, int page,
            int pageSize)
        {
            return new OkObjectResult(await _billService.GetAllPagingAsync(startDate, endDate, keyword, page, pageSize));
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(int billId, BillStatus status)
        {
            await _billService.UpdateStatusAsync(billId, status);
            return new OkResult();
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(BillViewModel billViewModel)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
            else
            {
                if (billViewModel.Id == 0)
                {
                    await _billService.CreateAsync(_mapper.Map<BillViewModel, Bill>(billViewModel));
                }
                else
                {
                    await _billService.UpdateAsync(_mapper.Map<BillViewModel, Bill>(billViewModel));
                }
                _billService.Save();
                return new OkObjectResult(billViewModel);
            }
        }

        [HttpGet]
        public IActionResult GetBillStatus()
        {
            List<EnumModel> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus))).Select(c => new EnumModel()
            {
                Value = (int)c,
                Name = c.ToString()
            }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpGet]
        public IActionResult GetPaymentMethod()
        {
            List<EnumModel> enums = ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod))).Select(c => new EnumModel()
            {
                Value = (int)c,
                Name = c.ToString()
            }).ToList();
            return new OkObjectResult(enums);
        }

        [HttpPost]
        public async Task<IActionResult> ExportExcel(int billId)
        {
            string webRootFolder = _hostingEnvironment.WebRootPath;
            string fileName = $"Bill_{billId}.xlsx";

            // template
            string templateDoc = Path.Combine(webRootFolder, "template", "BillTemplate.xlsx");
            string url = $"{Request.Scheme}://{Request.Host}/export-files/{fileName}";
            FileInfo file = new FileInfo(Path.Combine(webRootFolder, "export-files", fileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(webRootFolder, fileName));
            }

            await using (FileStream templateDocumentStream = System.IO.File.OpenRead(templateDoc))
            {
                using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                {
                    // add new worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Order"];

                    // load order header
                    var billDetail = await _billService.GetDetailAsync(billId);

                    // insert customer data into template
                    worksheet.Cells[4, 1].Value = "Tên khách hàng: " + billDetail.CustomerName;
                    worksheet.Cells[5, 1].Value = "Địa chỉ: " + billDetail.CustomerAddress;
                    worksheet.Cells[6, 1].Value = "SĐT: " + billDetail.CustomerMobile;

                    int rowIndex = 9;

                    // get order details
                    var orderDetails = await _billService.GetBillDetails(billId);
                    int count = 1;
                    foreach (var detail in orderDetails)
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

                    decimal total = (decimal)(orderDetails.Sum(x => x.Quantity * x.Price));
                    //worksheet.Cells[rowIndex + orderDetails.Count + 1, 5].Value = total.ToString("N0");
                    worksheet.Cells[24, 5].Value = total.ToString("N0");
                    var numberWord = "Tổng tiền bằng chữ : " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;
                    worksheet.Cells[28, 3].Value = billDetail.DateCreated.Day + "," + billDetail.DateCreated.Month +
                                                   "," + billDetail.DateCreated.Year;
                    await package.SaveAsAsync(file);
                }
            }
            return new OkObjectResult(url);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}