using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using eShop_Mvc.Core.Entities;
using eShop_Mvc.Core.Interfaces;
using eShop_Mvc.SharedKernel;
using eShop_Mvc.SharedKernel.Enums;
using eShop_Mvc.SharedKernel.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace eShop_Mvc.Core.Services
{
    public class BillService : IBillService
    {
        private readonly IRepository<Bill, int> _orderRepository;
        private readonly IRepository<BillDetail, int> _orderDetailRepository;
        private readonly IRepository<Product, int> _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BillService(IRepository<Bill, int> orderRepository, IRepository<BillDetail, int> orderDetailRepository,
                           IRepository<Product, int> productRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
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
    }
}