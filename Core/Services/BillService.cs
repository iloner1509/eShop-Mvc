using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.Core.Services.Query.BillQuery;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace eShop_Mvc.Core.Services
{
    public class BillService : IBillService
    {
        private readonly IRepository<Bill, int> _orderRepository;
        private readonly IRepository<BillDetail, int> _orderDetailRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IMediator _mediator;

        public BillService(IRepository<Bill, int> orderRepository, IRepository<BillDetail, int> orderDetailRepository, IHttpContextAccessor httpContextAccessor,
                           IRepository<Product, int> productRepository, IUnitOfWork unitOfWork, IHostingEnvironment hostingEnvironment, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _httpContextAccessor = httpContextAccessor;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
            _hostingEnvironment = hostingEnvironment;
            _mediator = mediator;
        }

        public async Task CreateAsync(Bill bill)
        {
            foreach (var billDetail in bill.BillDetails)
            {
                var product = await _productRepository.FindByIdAsync(billDetail.ProductId);
                billDetail.Price = product.Price;
            }

            await _orderRepository.AddAsync(bill);
        }

        public async Task UpdateAsync(Bill bill)
        {
            // get all order details
            var orderDetails = bill.BillDetails;

            // get newly added details
            var addedDetails = orderDetails.Where(x => x.Id == 0).ToList();

            // get updated details
            var updatedDetails = orderDetails.Where(x => x.Id != 0).ToList();

            // get existed details
            var existedDetails = _orderDetailRepository.FindAll(x => x.BillId == bill.Id);

            // clear all detail
            bill.BillDetails.Clear();
            foreach (var detail in updatedDetails)
            {
                var product = await _productRepository.FindByIdAsync(detail.ProductId);
                detail.Price = product.Price;
                await _orderDetailRepository.UpdateAsync(detail);
            }
            foreach (var detail in addedDetails)
            {
                var product = await _productRepository.FindByIdAsync(detail.ProductId);
                detail.Price = product.Price;
                await _orderDetailRepository.AddAsync(detail);
            }
            // delete old details
            var needDeleteDetail = existedDetails.AsEnumerable().Except(updatedDetails);
            _orderDetailRepository.DeleteMultipleAsync(needDeleteDetail);
            await _orderRepository.UpdateAsync(bill);
        }

        public async Task<Bill> GetDetailAsync(int billId)
        {
            var bill = await _orderRepository.FindSingleAsync(x => x.Id == billId);
            var billDetails = await _orderDetailRepository.FindAll(x => x.BillId == billId).ToListAsync();
            bill.BillDetails = billDetails;
            return bill;
        }

        public async Task<BillDetail> CreateBillDetailAsync(BillDetail billDetail)
        {
            await _orderDetailRepository.AddAsync(billDetail);
            return billDetail;
        }

        public async Task DeleteDetailAsync(int productId, int billId)
        {
            var detail =
                await _orderDetailRepository.FindSingleAsync(x => x.ProductId == productId && x.BillId == billId);
            await _orderDetailRepository.DeleteAsync(detail);
        }

        public async Task UpdateStatusAsync(int orderId, BillStatus status)
        {
            var order = await _orderRepository.FindByIdAsync(orderId);
            order.BillStatus = status;
            await _orderRepository.UpdateAsync(order);
        }

        public async Task<IReadOnlyList<BillDetail>> GetListBillDetailAsync(int billId)
        {
            return await _orderDetailRepository.FindAll(x => x.BillId == billId, o => o.Product).ToListAsync();
        }

        public async Task<PagedResult<Bill>> GetAllPagingAsync(string startDate, string endDate, string keyword, int page, int pageSize)
        {
            var query = _orderRepository.FindAll();
            if (!string.IsNullOrEmpty(startDate))
            {
                DateTime start = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated >= start);
            }

            if (!string.IsNullOrEmpty(endDate))
            {
                DateTime end = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.GetCultureInfo("vi-VN"));
                query = query.Where(x => x.DateCreated <= end);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.CustomerName.Contains(keyword) || x.CustomerMobile.Contains(keyword));
            }

            var totalRow = query.Count();
            var data = await query
                .OrderByDescending(x => x.DateCreated)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return new PagedResult<Bill>()
            {
                CurrentPage = page,
                PageSize = pageSize,
                Results = data,
                RowCount = totalRow
            };
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public async Task<string> ExportExcel(int billId)
        {
            string webRootFolder = _hostingEnvironment.WebRootPath;
            string fileName = $"Bill_{billId}.xlsx";

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
                    // add new worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets["Order"];

                    // load order header
                    var billDetail = await _mediator.Send(new GetBillDetailByIdQuery()
                    {
                        BillId = billId
                    });

                    // insert customer data into template
                    worksheet.Cells[4, 1].Value = "Tên khách hàng: " + billDetail.CustomerName;
                    worksheet.Cells[5, 1].Value = "Địa chỉ: " + billDetail.CustomerAddress;
                    worksheet.Cells[6, 1].Value = "SĐT: " + billDetail.CustomerMobile;

                    int rowIndex = 9;

                    // get order details
                    var orderDetails = await _mediator.Send(new GetListBillDetailByIdQuery()
                    {
                        BillId = billId
                    });
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

                    decimal total = orderDetails.Sum(x => x.Quantity * x.Price);
                    //worksheet.Cells[rowIndex + orderDetails.Count + 1, 5].Value = total.ToString("N0");
                    worksheet.Cells[24, 5].Value = total.ToString("N0");
                    var numberWord = "Tổng tiền bằng chữ : " + TextHelper.ToString(total);
                    worksheet.Cells[26, 1].Value = numberWord;
                    worksheet.Cells[28, 3].Value = billDetail.DateCreated.Day + "," + billDetail.DateCreated.Month +
                                                   "," + billDetail.DateCreated.Year;
                    await package.SaveAsAsync(file);
                }
            }

            return url;
        }
    }
}